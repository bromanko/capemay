namespace CapeMay.Admin.Domain.Commands

open System.Threading.Tasks
open CapeMay.Domain
open FlyIo
open CapeMay.Admin.Domain.FlyIo
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.TaskResult

module Deployments =
    type FlyIoVolume = { Name: string; Id: string }

    type FlyIoMachine = { Name: string; Id: string }

    type FlyIoApp =
        { OrgId: string
          Name: string
          Id: string
          Volume: FlyIoVolume option
          Machine: FlyIoMachine option }

    let ConsumerVolumeName = "db"
    let ConsumerMachineName = "default"

    let private createApp
        (client: FlyIoGraphqlClient)
        (orgId: NonEmptyString.T)
        (appName: NonEmptyString.T)
        : Task<Result<_, _>> =
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
                    Error
                        [ { message =
                              "App was not returned from createApp mutation." } ]
                | Some a ->
                    Ok
                        { OrgId = NonEmptyString.value orgId
                          Name = a.app.name
                          Id = a.app.id
                          Volume = None
                          Machine = None })
            Error

    let private parseApp (flyApp: GetApp.App) =
        let app =
            { OrgId = flyApp.organization.id
              Name = flyApp.name
              Id = flyApp.id
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

    let private upsertApp
        (client: FlyIoGraphqlClient)
        orgId
        appName
        (query: GetApp.Query)
        : Task<Result<FlyIoApp, ErrorType list>> =
        match query.app with
        | Some app -> parseApp app |> Ok |> Task.FromResult
        | None -> createApp client orgId appName

    let createVolume
        (client: FlyIoGraphqlClient)
        (app: FlyIoApp)
        volName
        : Task<Result<_, _>> =
        client.CreateVolumeAsync(
            { input =
                { clientMutationId = None
                  appId = app.Id
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
                    Error
                        [ { message =
                              "Volume was not returned from createVolume mutation." } ]
                | Some v ->
                    Ok
                        { app with
                            Volume =
                                Some
                                    { Id = v.volume.id
                                      Name = v.volume.name } })
            Error

    let upsertVolume
        (client: FlyIoGraphqlClient)
        (app: FlyIoApp)
        : Task<Result<FlyIoApp, ErrorType list>> =
        match app.Volume with
        | Some _ -> app |> Ok |> Task.FromResult
        | None -> createVolume client app ConsumerVolumeName


    let launchMachine
        (client: FlyIoGraphqlClient)
        config
        (app: FlyIoApp)
        : Task<Result<_, _>> =
        client.LaunchMachineAsync
            { input =
                { clientMutationId = None
                  appId = Some app.Id
                  organizationId = Some app.OrgId
                  id = None
                  name = None
                  region = Some PreferredRegion
                  config = config } }
        |> TaskResult.foldResult
            (fun q ->
                match q.launchMachine with
                | None ->
                    Error
                        [ { message =
                              "Machine was not returned from launchMachine mutation." } ]
                | Some m ->
                    Ok
                        { app with
                            Machine =
                                Some
                                    { Id = m.machine.id
                                      Name = m.machine.name } })
            Error


    let upsertMachine
        (client: FlyIoGraphqlClient)
        (appCfg: string)
        (app: FlyIoApp)
        : Task<Result<FlyIoApp, ErrorType list>> =
        match app.Machine with
        | Some _ -> app |> Ok |> Task.FromResult // this should do an update not a noop
        | None -> launchMachine client appCfg app


    let deployConsumerApp
        flyCfg
        appName
        appCfg
        : Task<Result<FlyIoApp, DomainError>> =
        let client = mkClient flyCfg

        client.GetAppAsync { appName = (NonEmptyString.value appName) }
        >>= upsertApp client flyCfg.OrgId appName
        >>= upsertVolume client
        >>= upsertMachine client appCfg
        |> TaskResult.mapError (fun errs ->
            List.map (fun (e: ErrorType) -> e.message) errs
            |> String.concat "\n"
            |> DomainError.UnhandledError)
