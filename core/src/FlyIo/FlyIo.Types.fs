namespace rec FlyIo

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type AddOnType =
    /// A Redis database
    | [<CompiledName "redis">] Redis

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type AppState =
    /// App has not been deployed
    | [<CompiledName "PENDING">] Pending
    /// App has been deployed
    | [<CompiledName "DEPLOYED">] Deployed
    /// App has been suspended
    | [<CompiledName "SUSPENDED">] Suspended

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type AutoscaleStrategy =
    /// autoscaling is disabled
    | [<CompiledName "NONE">] None
    /// place vms in preferred regions by weight
    | [<CompiledName "PREFERRED_REGIONS">] PreferredRegions
    /// place vms in regions near connection sources
    | [<CompiledName "CONNECTION_SOURCES">] ConnectionSources

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type BillingStatus =
    | [<CompiledName "CURRENT">] Current
    | [<CompiledName "SOURCE_REQUIRED">] SourceRequired
    | [<CompiledName "PAST_DUE">] PastDue

///All available http checks verbs
[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CheckHTTPVerb =
    | [<CompiledName "GET">] Get
    | [<CompiledName "HEAD">] Head

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type CheckType =
    /// tcp health check
    | [<CompiledName "TCP">] Tcp
    /// http health check
    | [<CompiledName "HTTP">] Http
    /// script health check
    | [<CompiledName "SCRIPT">] Script

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type DNSRecordChangeAction =
    /// A record should be created with the provided attributes
    | [<CompiledName "CREATE">] Create
    /// A record with the provided ID should be updated
    | [<CompiledName "UPDATE">] Update
    /// A record with the provided ID should be deleted
    | [<CompiledName "DELETE">] Delete

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type DNSRecordType =
    | [<CompiledName "A">] A
    | [<CompiledName "AAAA">] Aaaa
    | [<CompiledName "ALIAS">] Alias
    | [<CompiledName "CNAME">] Cname
    | [<CompiledName "MX">] Mx
    | [<CompiledName "NS">] Ns
    | [<CompiledName "SOA">] Soa
    | [<CompiledName "TXT">] Txt
    | [<CompiledName "SRV">] Srv

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type DeploymentStrategy =
    /// Deploy new instances all at once
    | [<CompiledName "IMMEDIATE">] Immediate
    /// Deploy new instances all at once
    | [<CompiledName "SIMPLE">] Simple
    /// Incrementally replace old instances with new ones
    | [<CompiledName "ROLLING">] Rolling
    /// Incrementally replace old instances with new ones, 1 by 1
    | [<CompiledName "ROLLING_ONE">] RollingOne
    /// Ensure new instances are healthy before continuing with a rolling deployment
    | [<CompiledName "CANARY">] Canary
    /// Launch all new instances before shutting down previous instances
    | [<CompiledName "BLUEGREEN">] Bluegreen

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type DomainDNSStatus =
    /// The DNS zone has not been created yet
    | [<CompiledName "PENDING">] Pending
    /// The DNS zone is being updated
    | [<CompiledName "UPDATING">] Updating
    /// The DNS zone is ready
    | [<CompiledName "READY">] Ready

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type DomainRegistrationStatus =
    /// The domain is not registered on fly
    | [<CompiledName "UNMANAGED">] Unmanaged
    /// The domain is being registered
    | [<CompiledName "REGISTERING">] Registering
    /// The domain is registered
    | [<CompiledName "REGISTERED">] Registered
    /// The domain is being transferred
    | [<CompiledName "TRANSFERRING">] Transferring
    /// The domain registration has expired
    | [<CompiledName "EXPIRED">] Expired

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type HTTPMethod =
    | [<CompiledName "GET">] Get
    | [<CompiledName "POST">] Post
    | [<CompiledName "PUT">] Put
    | [<CompiledName "PATCH">] Patch
    | [<CompiledName "HEAD">] Head
    | [<CompiledName "DELETE">] Delete

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type HTTPProtocol =
    /// HTTP protocol
    | [<CompiledName "HTTP">] Http
    /// HTTPS protocol
    | [<CompiledName "HTTPS">] Https

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type IPAddressType =
    | [<CompiledName "v4">] V4
    | [<CompiledName "v6">] V6
    | [<CompiledName "private_v6">] PrivateV6

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type OrganizationMemberRole =
    /// The user is an administrator of the organization
    | [<CompiledName "ADMIN">] Admin
    /// The user is a member of the organization
    | [<CompiledName "MEMBER">] Member

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type OrganizationTrust =
    /// We haven't set a trust level yet
    | [<CompiledName "UNKNOWN">] Unknown
    /// Organization has limited access to our service
    | [<CompiledName "RESTRICTED">] Restricted
    /// Organization cannot use our services
    | [<CompiledName "BANNED">] Banned
    /// Organization has to prove that is not fraud over time but can use our services
    | [<CompiledName "LOW">] Low
    /// Organization proved that it's safe to use our services
    | [<CompiledName "HIGH">] High

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type OrganizationType =
    /// A user's personal organization
    | [<CompiledName "PERSONAL">] Personal
    /// An organization shared between one or more users
    | [<CompiledName "SHARED">] Shared

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type PlatformVersionEnum =
    /// Nomad managed application
    | [<CompiledName "nomad">] Nomad
    /// App with only machines
    | [<CompiledName "machines">] Machines

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type RegionEnum =
    /// Amsterdam, NL
    | [<CompiledName "AMS">] Ams
    /// Atlanta, US
    | [<CompiledName "ATL">] Atl
    /// Dallas, US
    | [<CompiledName "DFW">] Dfw
    /// New York, US
    | [<CompiledName "EWR">] Ewr
    /// Frankfurt, DE
    | [<CompiledName "FRA">] Fra
    /// Hong Kong, CN
    | [<CompiledName "HKG">] Hkg
    /// Virginia, US
    | [<CompiledName "IAD">] Iad
    /// Los Angeles, US
    | [<CompiledName "LAX">] Lax
    /// Tokyo, JP
    | [<CompiledName "NRT">] Nrt
    /// Chicago, US
    | [<CompiledName "ORD">] Ord
    /// Seattle, US
    | [<CompiledName "SEA">] Sea
    /// Singapore, SG
    | [<CompiledName "SIN">] Sin
    /// San Jose, US
    | [<CompiledName "SJC">] Sjc
    /// Sydney, AU
    | [<CompiledName "SYD">] Syd
    /// Toronto, CA
    | [<CompiledName "YYZ">] Yyz

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type RuntimeType =
    /// Fly Container Runtime
    | [<CompiledName "FIRECRACKER">] Firecracker
    /// Fly JavaScript Runtime
    | [<CompiledName "NODEPROXY">] Nodeproxy

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type SchedulingEnum =
    /// Permanent app, restarts on exit
    | [<CompiledName "PERMANENT">] Permanent
    /// Ephemeral app, stays stopped on exit
    | [<CompiledName "EPHEMERAL">] Ephemeral

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ServiceHandlerType =
    /// Convert TLS connection to unencrypted TCP
    | [<CompiledName "TLS">] Tls
    /// Convert TCP connection to HTTP
    | [<CompiledName "HTTP">] Http
    /// Convert TCP connection to HTTP (at the edge)
    | [<CompiledName "EDGE_HTTP">] EdgeHttp
    /// Wrap TCP connection in PROXY protocol
    | [<CompiledName "PROXY_PROTO">] ProxyProto

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type ServiceProtocolType =
    /// TCP protocol
    | [<CompiledName "TCP">] Tcp
    /// UDP protocl
    | [<CompiledName "UDP">] Udp

[<Fable.Core.StringEnum; RequireQualifiedAccess>]
type VMSizeEnum =
    /// Shared 1x CPU core, 256MB
    | [<CompiledName "SHARED_CPU_1X">] SharedCpu1x
    /// Dedicated 1x CPU core, 2GB
    | [<CompiledName "DEDICATED_CPU_1X">] DedicatedCpu1x
    /// Dedicated 2x CPU core, 4GB
    | [<CompiledName "DEDICATED_CPU_2X">] DedicatedCpu2x
    /// Dedicated 4x CPU core, 8GB
    | [<CompiledName "DEDICATED_CPU_4X">] DedicatedCpu4x
    /// Dedicated 8x CPU core, 16GB
    | [<CompiledName "DEDICATED_CPU_8X">] DedicatedCpu8x

/// Autogenerated input type of AddWireGuardPeer
type AddWireGuardPeerInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The region in which to deploy the peer
      region: Option<string>
      /// The name with which to refer to the peer
      name: string
      /// The 25519 public key for the peer
      pubkey: string
      /// Network ID to attach wireguard peer to
      network: Option<string>
      /// Add via NATS transaction (deprecated - nats is always used)
      nats: Option<bool> }

/// Autogenerated input type of AllocateIPAddress
type AllocateIPAddressInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The application to allocate the ip address for
      appId: string
      /// The type of IP address to allocate (v4, v6, or private_v6)
      ``type``: IPAddressType
      /// Desired IP region (defaults to global)
      region: Option<string> }

/// Autogenerated input type of AttachPostgresCluster
type AttachPostgresClusterInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The postgres cluster application id
      postgresClusterAppId: string
      /// The application to attach postgres to
      appId: string
      /// The database to attach. Defaults to a new database with the same name as the app.
      databaseName: Option<string>
      /// The database user to create. Defaults to using the database name.
      databaseUser: Option<string>
      /// The environment variable name to set the connection string to. Defaults to DATABASE_URL
      variableName: Option<string>
      /// Flag used to indicate that flyctl will exec calls
      manualEntry: Option<bool> }

/// Region autoscaling configuration
type AutoscaleRegionConfigInput =
    { /// The region code to configure
      code: string
      /// The weight
      weight: Option<int>
      /// Minimum number of VMs to run in this region
      minCount: Option<int>
      /// Reset the configuration for this region
      reset: Option<bool> }

type BuildFinalImageInput =
    { /// Sha256 id of docker image
      id: string
      /// Tag used for docker image
      tag: string
      /// Size in bytes of the docker image
      sizeBytes: string }

type BuildImageOptsInput =
    { /// Path to dockerfile, if one exists
      dockerfilePath: Option<string>
      /// Unused in cli?
      imageRef: Option<string>
      /// Set of build time variables passed to cli
      buildArgs: Option<string>
      /// Unused in cli?
      extraBuildArgs: Option<string>
      /// Image label to use when tagging and pushing to the fly registry
      imageLabel: Option<string>
      /// Whether publishing to the registry was requested
      publish: Option<bool>
      /// Docker tag used to publish image to registry
      tag: Option<string>
      /// Set the target build stage to build if the Dockerfile has more than one stage
      target: Option<string>
      /// Do not use the build cache when building the image
      noCache: Option<bool>
      /// Builtin builder to use
      builtIn: Option<string>
      /// Builtin builder settings
      builtInSettings: Option<string>
      /// Fly.toml build.builder setting
      builder: Option<string>
      /// Fly.toml build.buildpacks setting
      buildPacks: Option<list<string>> }

type BuildStrategyAttemptInput =
    { /// Build strategy attempted
      strategy: string
      /// Result attempting this strategy
      result: string
      /// Optional error message from strategy
      error: Option<string>
      /// Optional note about this strategy or its result
      note: Option<string> }

type BuildTimingsInput =
    { /// Time to build and push the image, measured by flyctl
      buildAndPushMs: Option<string>
      /// Time to initialize client used to connect to either remote or local builder
      builderInitMs: Option<string>
      /// Time to build the image including create context, measured by flyctl
      buildMs: Option<string>
      /// Time to create the build context tar file, measured by flyctl
      contextBuildMs: Option<string>
      /// Time for builder to build image after receiving context, measured by flyctl
      imageBuildMs: Option<string>
      /// Time to push completed image to registry, measured by flyctl
      pushMs: Option<string> }

type BuilderMetaInput =
    { /// Local or remote builder type
      builderType: string
      /// Docker version reported by builder
      dockerVersion: Option<string>
      /// Whther or not buildkit is enabled on builder
      buildkitEnabled: Option<bool>
      /// Platform reported by the builder
      platform: Option<string>
      /// Remote builder app used
      remoteAppName: Option<string>
      /// Remote builder machine used
      remoteMachineId: Option<string> }

/// Autogenerated input type of CheckCertificate
type CheckCertificateInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// Application to ID
      appId: string
      /// Certificate hostname to check
      hostname: string }

/// Autogenerated input type of CheckDomain
type CheckDomainInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// Domain name to check
      domainName: string }

type CheckHeaderInput = { name: string; value: string }

type CheckInput =
    { ``type``: CheckType
      name: Option<string>
      /// Check interval in milliseconds
      interval: Option<int>
      /// Check timeout in milliseconds
      timeout: Option<int>
      httpMethod: Option<HTTPMethod>
      httpPath: Option<string>
      httpProtocol: Option<HTTPProtocol>
      httpTlsSkipVerify: Option<bool>
      httpHeaders: Option<list<CheckHeaderInput>>
      scriptCommand: Option<string>
      scriptArgs: Option<list<string>> }

/// health check state
type CheckJobHTTPOptionsInput =
    { verb: CheckHTTPVerb
      headers: Option<list<string>> }

/// Autogenerated input type of ConfigureRegions
type ConfigureRegionsInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// Regions to allow running in
      allowRegions: Option<list<string>>
      /// Regions to deny running in
      denyRegions: Option<list<string>>
      /// Fallback regions. Used if preferred regions are having issues
      backupRegions: Option<list<string>>
      /// Process group to modify
      group: Option<string> }

/// Autogenerated input type of CreateAddOn
type CreateAddOnInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// An optional application ID to attach the add-on to after provisioning
      appId: Option<string>
      /// The organization which owns the add-on
      organizationId: Option<string>
      /// The add-on type to provision
      ``type``: AddOnType
      /// An optional name for the add-on
      name: Option<string>
      /// The add-on plan ID
      planId: string
      /// Desired primary region for the add-on
      primaryRegion: string
      /// Desired regions to place replicas in
      readRegions: Option<list<string>>
      /// Options specific to the add-on
      options: Option<string> }

/// Autogenerated input type of CreateAndRegisterDomain
type CreateAndRegisterDomainInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The domain name
      name: string
      /// Enable whois privacy on the registration
      whoisPrivacy: Option<bool>
      /// Enable auto renew on the registration
      autoRenew: Option<bool> }

/// Autogenerated input type of CreateAndTransferDomain
type CreateAndTransferDomainInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The domain name
      name: string
      /// The authorization code
      authorizationCode: string }

/// Autogenerated input type of CreateApp
type CreateAppInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The application runtime
      runtime: Option<RuntimeType>
      /// The name of the new application. Defaults to a random name.
      name: Option<string>
      preferredRegion: Option<string>
      heroku: Option<bool>
      network: Option<string>
      appRoleId: Option<string>
      machines: Option<bool> }

/// Autogenerated input type of CreateBuild
type CreateBuildInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The name of the app being built
      appName: string
      /// The ID of the machine being built (only set for machine builds)
      machineId: Option<string>
      /// Options set for building image
      imageOpts: BuildImageOptsInput
      /// List of available build strategies that will be attempted
      strategiesAvailable: list<string>
      /// Whether builder is remote or local
      builderType: string }

/// Autogenerated input type of CreateCheckJob
type CreateCheckJobInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// Organization ID
      organizationId: string
      /// The URL to check
      url: string
      /// http checks locations
      locations: list<string>
      /// http check options
      httpOptions: CheckJobHTTPOptionsInput }

/// Autogenerated input type of CreateCheckJobRun
type CreateCheckJobRunInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// Check Job ID
      checkJobId: string }

/// Autogenerated input type of CreateDNSPortal
type CreateDNSPortalInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The unique name of this portal. A random name will be generated if omitted.
      name: Option<string>
      /// The title of this portal
      title: Option<string>
      /// The return url for this portal
      returnUrl: Option<string>
      /// The text to display for the return url link
      returnUrlText: Option<string>
      /// The support url for this portal
      supportUrl: Option<string>
      /// The text to display for the support url link
      supportUrlText: Option<string>
      /// The primary branding color
      primaryColor: Option<string>
      /// The secondary branding color
      accentColor: Option<string> }

/// Autogenerated input type of CreateDNSPortalSession
type CreateDNSPortalSessionInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the dns portal
      dnsPortalId: string
      /// The node ID of the domain to edit
      domainId: string
      /// Optionally override the portal's default title for this session
      title: Option<string>
      /// Optionally override the portal's default return url for this session
      returnUrl: Option<string>
      /// Optionally override the portal's default return url text for this session
      returnUrlText: Option<string> }

/// Autogenerated input type of CreateDNSRecord
type CreateDNSRecordInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the domain
      domainId: string
      /// The type of the record
      ``type``: DNSRecordType
      /// The dns record name
      name: string
      /// The TTL in seconds
      ttl: int
      /// The content of the record
      rdata: string }

/// Autogenerated input type of CreateDelegatedWireGuardToken
type CreateDelegatedWireGuardTokenInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The name with which to refer to the peer
      name: Option<string> }

/// Autogenerated input type of CreateDoctorReport
type CreateDoctorReportInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The report data
      data: string }

/// Autogenerated input type of CreateDomain
type CreateDomainInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The domain name
      name: string }

/// Autogenerated input type of CreateOrganization
type CreateOrganizationInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The name of the organization
      name: string }

/// Autogenerated input type of CreateOrganizationInvitation
type CreateOrganizationInvitationInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The email to invite
      email: string }

/// Autogenerated input type of CreatePostgresClusterDatabase
type CreatePostgresClusterDatabaseInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The name of the postgres cluster app
      appName: string
      /// The name of the database
      databaseName: string }

/// Autogenerated input type of CreatePostgresCluster
type CreatePostgresClusterInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The name of the new application. Defaults to a random name.
      name: Option<string>
      region: Option<string>
      /// The superuser password. Defaults to a random password.
      password: Option<string>
      /// The VM size to use. Defaults to shared-cpu-1x
      vmSize: Option<string>
      /// The volume size in GB. Defaults to 10.
      volumeSizeGb: Option<int>
      /// Number of VM's to provision
      count: Option<int>
      imageRef: Option<string>
      snapshotId: Option<string> }

/// Autogenerated input type of CreatePostgresClusterUser
type CreatePostgresClusterUserInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The name of the postgres cluster app
      appName: string
      /// The name of the database
      username: string
      /// The password of the user
      password: string
      /// Should this user be a superuser
      superuser: Option<bool> }

/// Autogenerated input type of CreateTemplateDeployment
type CreateTemplateDeploymentInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization to move the app to
      organizationId: string
      template: string
      variables: Option<list<PropertyInput>> }

/// Autogenerated input type of CreateVolume
type CreateVolumeInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The application to attach the new volume to
      appId: string
      /// Volume name
      name: string
      /// Desired region for volume
      region: string
      /// Desired volume size, in GB
      sizeGb: int
      /// Volume should be encrypted at rest
      encrypted: Option<bool>
      /// Provision volume in a redundancy zone not already in use by this app
      requireUniqueZone: Option<bool>
      snapshotId: Option<string> }

/// Autogenerated input type of CreateVolumeSnapshot
type CreateVolumeSnapshotInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      volumeId: string }

type DNSRecordChangeInput =
    { /// The action to perform on this record.
      action: DNSRecordChangeAction
      /// The id of the record this action will apply to. This is required if the action is UPDATE or DELETE.
      recordId: Option<string>
      /// The record type. This is required if action is CREATE.
      ``type``: Option<DNSRecordType>
      /// The name of the record. If omitted it will default to @ - the zone apex.
      name: Option<string>
      /// The number of seconds this record can be cached for. Defaults to 1 hour.
      ttl: Option<int>
      /// The record data. Required if action is CREATE
      rdata: Option<string> }

/// Autogenerated input type of DeleteAddOn
type DeleteAddOnInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the add-on to delete
      addOnId: Option<string>
      /// The name of the add-on to delete
      name: Option<string> }

/// Autogenerated input type of DeleteDNSPortal
type DeleteDNSPortalInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the dns portal
      dnsPortalId: string }

/// Autogenerated input type of DeleteDNSPortalSession
type DeleteDNSPortalSessionInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the dns portal session
      dnsPortalSessionId: string }

/// Autogenerated input type of DeleteDNSRecord
type DeleteDNSRecordInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the DNS record
      recordId: string }

/// Autogenerated input type of DeleteDelegatedWireGuardToken
type DeleteDelegatedWireGuardTokenInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The raw WireGuard token
      token: Option<string>
      /// The name with which to refer to the token
      name: Option<string> }

/// Autogenerated input type of DeleteDeploymentSource
type DeleteDeploymentSourceInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The application to update
      appId: string }

/// Autogenerated input type of DeleteDomain
type DeleteDomainInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the domain
      domainId: string }

/// Autogenerated input type of DeleteHealthCheckHandler
type DeleteHealthCheckHandlerInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// Handler name
      name: string }

/// Autogenerated input type of DeleteOrganization
type DeleteOrganizationInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the organization to delete
      organizationId: string }

/// Autogenerated input type of DeleteOrganizationInvitation
type DeleteOrganizationInvitationInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the invitation
      invitationId: string }

/// Autogenerated input type of DeleteOrganizationMembership
type DeleteOrganizationMembershipInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The node ID of the user
      userId: string }

/// Autogenerated input type of DeleteRemoteBuilder
type DeleteRemoteBuilderInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string }

/// Autogenerated input type of DeleteVolume
type DeleteVolumeInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the volume
      volumeId: string }

/// Autogenerated input type of DeployImage
type DeployImageInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// The image to deploy
      image: string
      /// Network services to expose
      services: Option<list<ServiceInput>>
      /// app definition
      definition: Option<string>
      /// The strategy for replacing existing instances. Defaults to canary.
      strategy: Option<DeploymentStrategy> }

/// Autogenerated input type of DetachPostgresCluster
type DetachPostgresClusterInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The postgres cluster application id
      postgresClusterAppId: string
      /// The application to detach postgres from
      appId: string
      /// The postgres attachment id
      postgresClusterAttachmentId: Option<string> }

/// Autogenerated input type of DummyWireGuardPeer
type DummyWireGuardPeerInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The region in which to deploy the peer
      region: Option<string> }

/// Autogenerated input type of EnablePostgresConsul
type EnablePostgresConsulInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: Option<string> }

/// Autogenerated input type of EnsureMachineRemoteBuilder
type EnsureMachineRemoteBuilderInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The unique application name
      appName: Option<string>
      /// The node ID of the organization
      organizationId: Option<string>
      /// Desired region for the remote builder
      region: Option<string>
      /// Use v2 machines
      v2: Option<bool> }

/// Autogenerated input type of EstablishSSHKey
type EstablishSSHKeyInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// Establish a key even if one is already set
      ``override``: Option<bool> }

/// Autogenerated input type of ExportDNSZone
type ExportDNSZoneInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// ID of the domain to export
      domainId: string }

/// Autogenerated input type of ExtendVolume
type ExtendVolumeInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the volume
      volumeId: string
      /// The target volume size
      sizeGb: int }

/// Autogenerated input type of FinishBuild
type FinishBuildInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// Build id returned by createBuild() mutation
      buildId: string
      /// The name of the app being built
      appName: string
      /// The ID of the machine being built (only set for machine builds)
      machineId: Option<string>
      /// Indicate whether build completed or failed
      status: string
      /// Build strategies attempted and their result, should be in order of attempt
      strategiesAttempted: Option<list<BuildStrategyAttemptInput>>
      /// Metadata about the builder
      builderMeta: Option<BuilderMetaInput>
      /// Information about the docker image that was built
      finalImage: Option<BuildFinalImageInput>
      /// Timings for different phases of the build
      timings: Option<BuildTimingsInput>
      /// Log or error output
      logs: Option<string> }

/// Autogenerated input type of GrantPostgresClusterUserAccess
type GrantPostgresClusterUserAccessInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The name of the postgres cluster app
      appName: string
      /// The name of the database
      username: string
      /// The database to grant access to
      databaseName: string }

/// Autogenerated input type of ImportDNSZone
type ImportDNSZoneInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// ID of the domain to export
      domainId: string
      zonefile: string }

/// Autogenerated input type of IssueCertificate
type IssueCertificateInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// Email address of user to be issued certificate
      email: string
      /// Unix username valid for certificate
      username: Option<string>
      /// Hours for which certificate will be valid
      validHours: Option<int>
      /// Comma-separated list of SSH principals for certificate
      principals: Option<string> }

/// Autogenerated input type of KillMachine
type KillMachineInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: Option<string>
      /// machine id
      id: string }

/// Autogenerated input type of LaunchApp
type LaunchAppInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// App name to create
      name: Option<string>
      /// Configuration
      config: Option<string>
      /// Size of VM for your app
      vmSize: Option<VMSizeEnum>
      /// Regions to launch your app to
      regions: Option<list<string>>
      /// Number of instances to run
      count: Option<int>
      /// Volumes to create
      volumes: Option<list<VolumeInput>>
      /// Secrets to set
      secrets: Option<list<SecretInput>>
      /// Type of scheduling for this app
      scheduling: Option<SchedulingEnum>
      /// Docker image to launch
      image: string }

/// Autogenerated input type of LaunchMachine
type LaunchMachineInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: Option<string>
      /// The node ID of the organization
      organizationId: Option<string>
      /// The ID of the machine
      id: Option<string>
      /// The name of the machine
      name: Option<string>
      /// Region for the machine
      region: Option<string>
      /// Configuration
      config: string }

/// Autogenerated input type of MoveApp
type MoveAppInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The application to move
      appId: string
      /// The node ID of the organization to move the app to
      organizationId: string }

/// Autogenerated input type of PauseApp
type PauseAppInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string }

type PropertyInput =
    { /// The name of the property
      name: string
      /// The value of the property
      value: Option<string> }

/// Autogenerated input type of RegisterDomain
type RegisterDomainInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the domain
      domainId: string
      /// Enable whois privacy on the registration
      whoisPrivacy: Option<bool>
      /// Enable auto renew on the registration
      autoRenew: Option<bool> }

/// Autogenerated input type of ReleaseIPAddress
type ReleaseIPAddressInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The id of the ip address to release
      ipAddressId: string }

/// Autogenerated input type of RemoveMachine
type RemoveMachineInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: Option<string>
      /// machine id
      id: string
      /// force kill machine if it's running
      kill: Option<bool> }

/// Autogenerated input type of RemoveWireGuardPeer
type RemoveWireGuardPeerInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The name of the peer to remove
      name: string
      /// Add via NATS transaction (for testing only, nosy users)
      nats: Option<bool> }

/// Autogenerated input type of RestartAllocation
type RestartAllocationInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// The ID of the app
      allocId: string }

/// Autogenerated input type of RestartApp
type RestartAppInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string }

/// Autogenerated input type of RestoreVolumeSnapshot
type RestoreVolumeSnapshotInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      volumeId: string
      snapshotId: string }

/// Autogenerated input type of ResumeApp
type ResumeAppInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string }

/// Autogenerated input type of RevokePostgresClusterUserAccess
type RevokePostgresClusterUserAccessInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The name of the postgres cluster app
      appName: string
      /// The username to revoke
      username: string
      /// The database to revoke access to
      databaseName: string }

/// Autogenerated input type of SaveDeploymentSource
type SaveDeploymentSourceInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The application to update
      appId: string
      provider: string
      repositoryId: string
      ref: Option<string>
      baseDir: Option<string>
      skipBuild: Option<bool> }

/// Autogenerated input type of ScaleApp
type ScaleAppInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// Regions to scale
      regions: list<ScaleRegionInput> }

/// Region placement configuration
type ScaleRegionInput =
    { /// The region to configure
      region: string
      /// The value to change by
      count: int }

/// A secure configuration value
type SecretInput =
    { /// The unqiue key for this secret
      key: string
      /// The value of this secret
      value: string }

/// Global port routing
type ServiceInput =
    { /// Protocol to listen on
      protocol: ServiceProtocolType
      /// Ports to listen on
      ports: Option<list<ServiceInputPort>>
      /// Application port to forward traffic to
      internalPort: int
      /// Health checks
      checks: Option<list<CheckInput>>
      /// Soft concurrency limit
      softConcurrency: Option<int>
      /// Hard concurrency limit
      hardConcurrency: Option<int> }

/// Service port
type ServiceInputPort =
    { /// Port to listen on
      port: int
      /// Handlers to apply before forwarding service traffic
      handlers: Option<list<ServiceHandlerType>>
      /// tls options
      tlsOptions: Option<ServicePortTlsOptionsInput> }

/// TLS handshakes options for a port
type ServicePortTlsOptionsInput = { defaultSelfSigned: Option<bool> }

/// Autogenerated input type of SetPagerdutyHandler
type SetPagerdutyHandlerInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// Handler name
      name: string
      /// PagerDuty API token
      pagerdutyToken: string
      /// Map of alert severity levels to PagerDuty severity levels
      pagerdutyStatusMap: Option<string> }

/// Autogenerated input type of SetSecrets
type SetSecretsInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// Secrets to set
      secrets: list<SecretInput>
      /// By default, we set only the secrets you specify. Set this to true to replace all secrets.
      replaceAll: Option<bool> }

/// Autogenerated input type of SetSlackHandler
type SetSlackHandlerInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// Handler name
      name: string
      /// Slack Webhook URL to use for health check notifications
      slackWebhookUrl: string
      /// Slack channel to send messages to, defaults to #general
      slackChannel: Option<string>
      /// User name to display on Slack Messages (defaults to Fly)
      slackUsername: Option<string>
      /// Icon to show with Slack messages
      slackIconUrl: Option<string> }

/// Autogenerated input type of SetVMCount
type SetVMCountInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// Counts for VM groups
      groupCounts: list<VMCountInput> }

/// Autogenerated input type of SetVMSize
type SetVMSizeInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// The name of the vm size to set
      sizeName: string
      /// Optionally request more memory
      memoryMb: Option<int>
      /// Process group to modify
      group: Option<string> }

/// Autogenerated input type of StartBuild
type StartBuildInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string }

/// Autogenerated input type of StartMachine
type StartMachineInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: Option<string>
      /// machine id
      id: string }

/// Autogenerated input type of StopAllocation
type StopAllocationInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// The ID of the app
      allocId: string }

/// Autogenerated input type of StopMachine
type StopMachineInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: Option<string>
      /// machine id
      id: string
      /// signal to send the machine
      signal: Option<string>
      /// how long to wait before force killing the machine
      killTimeoutSecs: Option<int> }

/// Autogenerated input type of UnsetSecrets
type UnsetSecretsInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      /// Secret keys to unset
      keys: list<string> }

/// Autogenerated input type of UpdateAddOn
type UpdateAddOnInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The add-on ID to update
      addOnId: Option<string>
      /// The add-on name to update
      name: Option<string>
      /// The add-on plan ID
      planId: Option<string>
      /// Desired regions to place replicas in
      readRegions: Option<list<string>> }

/// Autogenerated input type of UpdateAutoscaleConfig
type UpdateAutoscaleConfigInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The ID of the app
      appId: string
      enabled: Option<bool>
      minCount: Option<int>
      maxCount: Option<int>
      balanceRegions: Option<bool>
      /// Region configs
      regions: Option<list<AutoscaleRegionConfigInput>>
      resetRegions: Option<bool> }

/// Autogenerated input type of UpdateDNSPortal
type UpdateDNSPortalInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      dnsPortalId: string
      /// The unique name of this portal.
      name: Option<string>
      /// The title of this portal
      title: Option<string>
      /// The return url for this portal
      returnUrl: Option<string>
      /// The text to display for the return url link
      returnUrlText: Option<string>
      /// The support url for this portal
      supportUrl: Option<string>
      /// The text to display for the support url link
      supportUrlText: Option<string>
      /// The primary branding color
      primaryColor: Option<string>
      /// The secondary branding color
      accentColor: Option<string> }

/// Autogenerated input type of UpdateDNSRecord
type UpdateDNSRecordInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the DNS record
      recordId: string
      /// The dns record name
      name: Option<string>
      /// The TTL in seconds
      ttl: Option<int>
      /// The content of the record
      rdata: Option<string> }

/// Autogenerated input type of UpdateDNSRecords
type UpdateDNSRecordsInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the domain
      domainId: string
      changes: list<DNSRecordChangeInput> }

/// Autogenerated input type of UpdateOrganizationMembership
type UpdateOrganizationMembershipInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// The node ID of the user
      userId: string
      /// The new role for the user
      role: OrganizationMemberRole }

/// Autogenerated input type of UpdateRemoteBuilder
type UpdateRemoteBuilderInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      /// The node ID of the organization
      organizationId: string
      /// Docker image reference
      image: string }

type VMCountInput =
    { /// VM group name
      group: Option<string>
      /// The desired count
      count: Option<int>
      /// Max number of VMs to allow per region
      maxPerRegion: Option<int> }

/// Autogenerated input type of ValidateWireGuardPeers
type ValidateWireGuardPeersInput =
    { /// A unique identifier for the client performing the mutation.
      clientMutationId: Option<string>
      peerIps: list<string> }

type VolumeInput =
    { /// Volume name
      name: string
      /// Desired region for volume
      region: RegionEnum
      /// Desired volume size, in GB
      sizeGb: int
      /// Volume should be encrypted at rest
      encrypted: Option<bool>
      /// How many volumes to create with this configuration
      count: Option<int> }

/// The error returned by the GraphQL backend
type ErrorType = { message: string }
