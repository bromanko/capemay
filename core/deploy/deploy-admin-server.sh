#!/usr/bin/env bash

set -eo pipefail

FLY_API_HOSTNAME="localhost:4280"
FLY_REGION="${FLY_REGION:-sea}"

if [[ -z "$FLY_API_TOKEN" ]]; then
    echo "FLY_API_TOKEN environment variable must be set." 1>&2
    exit 1
fi
if [[ -z "$FLY_API_HOSTNAME" ]]; then
    echo "FLY_API_HOSTNAME environment variable must be set." 1>&2
    exit 1
fi

if [[ -z "$1" ]]; then
    echo "Must provide org slug as first argument." 1>&2
    exit 1
fi
org=$1

if [[ -z "$2" ]]; then
    echo "Must provide app name as second argument." 1>&2
    exit 1
fi
app=$2

set -u

proxy_pid=""
startProxy() {
    echo "Starting proxy..."
    fly machines api-proxy &
    proxy_pid=$!
}

stopProxy() {
    if [ -n "$proxy_pid" ]; then
        kill "$proxy_pid"
    fi
}

appExists() {
    local org="$1"
    local app="$2"

    # TODO - Filter by org
    fly apps list -j | jq -e "any(.[].Name; . == \"${app}\")" >/dev/null
}

createApp() {
    local org="$1"
    local app="$2"

    fly apps create "${app}" --machines -o "${org}"
}

volumeExists() {
    local app="${1}"

    fly volumes list -a "${app}" -j |
        jq -e '.. | select(type == "array" and length > 0)' >/dev/null
}

createVolume() {
    local app="${1}"

    fly volumes create db -a "${app}" -r "${FLY_REGION}"
}

pushImage() {
    local app="${1}"
    local label="${2}"

    fly deploy --build-only --push \
        -a "${app}" --dockerfile src/CapeMay.Admin.Server/Dockerfile \
        --image-label "${label}"
}

machineExists() {
    local app="$1"

    fly machines list -a "${app}" -j |
        jq -e '.. | select(type == "array" and length > 0)' >/dev/null
}

machineConfig() {
    local app="$1"
    local label="$2"
    local machine_name="$3"
    local volume_id="$4"

    cat <<EOF
{
  "name": "${machine_name}",
  "config": {
      "image": "registry.fly.io/${app}:${label}",
      "env": {
        "ADMIN_DB_CONNECTION_STRING": "Data Source=/db/admin.sqlite;Mode=ReadWrite;Cache=Shared;Foreign Keys=True;",
        "ADMIN_SERVER_URL": "http://*:8080"
      },
      "services": [{
        "ports": [
          {
            "port": 80,
            "handlers": ["http"],
            "force_https": true
          },
          {
            "port": 443,
            "handlers": ["http", "tls"]
          }
        ],
        "protocol": "tcp",
        "internal_port": 8080
      }],
      "mounts": [{
        "volume": "${volume_id}",
        "path": "/db"
      }]
  }
}
EOF
}

createMachine() {
    local app="$1"
    local label="$2"
    local machine_name="$3"
    local volume_name="$4"

    local volume_id
    volume_id=$(fly volume list -a "${app}" -j | jq -r "select(.[].Name == \"${volume_name}\")[0].id")

    local body
    body=$(machineConfig "$app" "$label" "$machine_name" "$volume_id")
    curl -i -X POST \
        -H "Authorization: Bearer ${FLY_API_TOKEN}" -H "Content-Type: application/json" \
        "http://${FLY_API_HOSTNAME}/v1/apps/${app}/machines" \
        -d "${body}"
}

updateMachine() {
    local app="$1"
    local label="$2"
    local machine_name="$3"
    local volume_name="$4"

    local volume_id
    volume_id=$(fly volume list -a "${app}" -j | jq -r "select(.[].Name == \"${volume_name}\")[0].id")

    local machine_id
    machine_id=$(fly machines list -a "${app}" -j | jq -r "select(.[].name == \"${machine_name}\")[0].id")

    local body
    body=$(machineConfig "$app" "$label" "$machine_name" "$volume_id")
    curl -i -X POST \
        -H "Authorization: Bearer ${FLY_API_TOKEN}" -H "Content-Type: application/json" \
        "http://${FLY_API_HOSTNAME}/v1/apps/${app}/machines/${machine_id}" \
        -d "${body}"
}

startProxy
trap stopProxy EXIT

if appExists "${org}" "$app"; then
    echo "App \"${app}\" exists. Skipping creation."
else
    echo "Creating app \"${app}\"..."
    createApp "${org}" "$app"
fi

if volumeExists "$app"; then
    echo "Volume exists. Skipping creation."
else
    echo "Creating volume..."
    createVolume "${app}"
fi

label=$(git rev-parse --short HEAD)
machine_name="default"
volume_name="db"

echo "Building and pushing image..."
pushImage "${app}" "${label}"

if machineExists "$app"; then
    echo "Machine exists. Updating..."
    updateMachine "${app}" "${label}" "${machine_name}" "${volume_name}"
else
    echo "Creating machine..."
    createMachine "${app}" "${label}" "${machine_name}" "${volume_name}"
fi
