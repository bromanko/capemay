namespace CapeMay.Admin.Domain.Commands

open System.Threading.Tasks
open CapeMay.Domain
open FSharpx
open FlyIo
open CapeMay.Admin.Domain.FlyIo
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.TaskResult

module Deployments =
    type FlyIoApp =
        { OrgId: string
          Name: string
          Id: string }

    type FlyIoVolume = { Name: string; Id: string }

    type FlyIoMachine = { Name: string; Id: string }

    type ParsedFlyIoApp =
        { App: FlyIoApp
          Volume: FlyIoVolume option
          Machine: FlyIoMachine option }

    let ConsumerVolumeName = "db"

    let ConsumerMachineName = "default"

    let private tryGetApp
        (client: FlyIoGraphqlClient)
        (appName: NonEmptyString.T)
        : Task<Result<GetApp.App option, _>> =
        client.GetAppAsync { appName = (NonEmptyString.value appName) }
        |> TaskResult.foldResult
            (fun q ->
                match q.app with
                | None ->
                    mkFlyIoError
                        "App was not returned from getApp query even though it exists."
                    |> Error
                | Some app -> Some app |> Ok)
            (fun e -> if hasError e "NOT_FOUND" then Ok None else Error e)

    let private parseApp (flyApp: GetApp.App) : ParsedFlyIoApp =
        let app =
            { App =
                { OrgId = flyApp.organization.id
                  Name = flyApp.name
                  Id = flyApp.id }
              Volume = None
              Machine = None }

        let app =
            match flyApp.volumes.nodes with
            | Some vols ->
                match
                    List.tryFind
                        (fun (v: GetApp.Volume option) ->
                            v.IsSome && v.Value.name = ConsumerVolumeName)
                        vols
                with
                | Some (Some v) ->
                    { app with
                        Volume = Some { FlyIoVolume.Id = v.id; Name = v.name } }
                | _ -> app
            | _ -> app

        match flyApp.machines.nodes with
        | Some machines ->
            match
                List.tryFind
                    (fun (m: GetApp.Machine option) ->
                        m.IsSome && m.Value.name = ConsumerMachineName)
                    machines
            with
            | Some (Some m) ->
                { app with Machine = Some { Id = m.id; Name = m.name } }
            | _ -> app
        | _ -> app

    let private createApp
        (client: FlyIoGraphqlClient)
        (orgId: NonEmptyString.T)
        (appName: NonEmptyString.T)
        : Task<Result<FlyIoApp, _>> =
        client.CreateAppAsync(
            { input =
                { clientMutationId = None
                  organizationId = NonEmptyString.value orgId
                  runtime = None
                  name = NonEmptyString.value appName |> Some
                  preferredRegion = Some PreferredRegion
                  heroku = Some false
                  network = None
                  appRoleId = None
                  machines = Some true } }
        )
        |> TaskResult.foldResult
            (fun a ->
                match a.createApp with
                | None ->
                    mkFlyIoError "App was not returned from createApp mutation."
                    |> Error
                | Some a ->
                    Ok
                        { OrgId = NonEmptyString.value orgId
                          Name = a.app.name
                          Id = a.app.id })
            Error

    let private upsertApp
        (client: FlyIoGraphqlClient)
        orgId
        appName
        : Task<Result<ParsedFlyIoApp, FlyIoError list>> =
        tryGetApp client appName
        >>= (fun result ->
            match result with
            | Some app -> parseApp app |> Ok |> Task.FromResult
            | None ->
                createApp client orgId appName
                |> TaskResult.map (fun app ->
                    { App = app
                      Volume = None
                      Machine = None }))

    let private createVolume
        (client: FlyIoGraphqlClient)
        (appId: string)
        volName
        : Task<Result<FlyIoVolume, _>> =
        client.CreateVolumeAsync(
            { input =
                { clientMutationId = None
                  appId = appId
                  name = volName
                  region = PreferredRegion
                  sizeGb = 3
                  encrypted = Some true
                  requireUniqueZone = None
                  snapshotId = None } }
        )
        |> TaskResult.foldResult
            (fun q ->
                match q.createVolume with
                | None ->
                    mkFlyIoError
                        "Volume was not returned from createVolume mutation."
                    |> Error
                | Some v ->
                    Ok
                        { Id = v.volume.id
                          Name = v.volume.name })
            Error

    let private upsertVolume
        (client: FlyIoGraphqlClient)
        (parsed: ParsedFlyIoApp)
        : Task<Result<FlyIoVolume, FlyIoError list>> =
        match parsed.Volume with
        | Some v -> v |> Ok |> Task.FromResult
        | None -> createVolume client parsed.App.Id ConsumerVolumeName

    let private createMachine
        (client: FlyIoRestClient)
        config
        (app: FlyIoApp)
        : Task<Result<FlyIoMachine, _>> =
        client.CreateMachine app.Name ConsumerMachineName config
        |> TaskResult.map (fun m -> { Id = m.id; Name = m.name })

    let private updateMachine
        (client: FlyIoRestClient)
        config
        (app: FlyIoApp)
        (machine: FlyIoMachine)
        : Task<Result<FlyIoMachine, _>> =
        client.UpdateMachine app.Name machine.Id config
        |> TaskResult.map (fun m -> { Id = m.id; Name = m.name })

    let private upsertMachine
        (client: FlyIoRestClient)
        machineCfgBuilder
        (parsed: ParsedFlyIoApp)
        (vol: FlyIoVolume)
        : Task<Result<FlyIoMachine, FlyIoError list>> =
        let machineConfig = machineCfgBuilder parsed.App vol

        match parsed.Machine with
        | Some m -> updateMachine client machineConfig parsed.App m
        | None -> createMachine client machineConfig parsed.App


    let deployConsumerApp
        flyGraphqlCfg
        flyRestCfg
        appName
        machineCfgBuilder
        : Task<Result<FlyIoApp, DomainError>> =
        let graphqlClient = mkGraphqlClient flyGraphqlCfg
        let restClient = mkRestClient flyRestCfg

        upsertApp graphqlClient flyGraphqlCfg.OrgId appName
        >>= (fun parsed ->
            upsertVolume graphqlClient parsed
            >>= upsertMachine restClient machineCfgBuilder parsed
            |> TaskResult.map (fun _ -> parsed.App))
        |> TaskResult.mapError (fun errs ->
            List.map (fun (e: FlyIoError) -> e.message) errs
            |> String.concat "\n"
            |> DomainError.UnhandledError)


    let mkConsumerMachineCfg (_: FlyIoApp) (vol: FlyIoVolume) =
        {| image = "flyio/fastify-functions"
           env = dict [ "CONSUMER_SERVER_URL", "http://*:8080" ]
           services =
            [ {| ports =
                  [ {| port = 80
                       handlers = [ "http" ]
                       force_https = true |}
                    {| port = 443
                       handlers = [ "http"; "tls" ]
                       force_https = false |} ]
                 protocol = "tcp"
                 internal_port = 8080 |} ]
           mounts = [ {| volume = vol.Id; path = "/db" |} ] |}
