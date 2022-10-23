FROM mcr.microsoft.com/dotnet/sdk:6.0 AS packages
WORKDIR /app

COPY .config ./
RUN dotnet tool restore

COPY paket.dependencies paket.lock CapeMay.sln ./
RUN dotnet paket restore

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY --from=packages /root/.nuget /root/.nuget

COPY . ./
RUN dotnet tool restore
RUN dotnet publish src/CapeMay.Admin.Server/CapeMay.Admin.Server.fsproj -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN apt-get -qq update \
    && apt-get -qq --no-install-recommends install sqitch

WORKDIR /app

COPY --from=build-env /app/out ./
COPY --from=build-env /app/db /db/
RUN mkdir -p /db/.run

ENTRYPOINT ["./CapeMay.Admin.Server"]
