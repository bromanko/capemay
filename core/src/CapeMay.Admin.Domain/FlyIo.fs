namespace CapeMay.Admin.Domain

open System
open System.Collections.Generic
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open CapeMay.Domain
open FlyIo
open FsToolkit.ErrorHandling
open FsHttp
open FsHttp.NewtonsoftJson

module FlyIo =
    type ClientConfig =
        { BaseAddress: Uri
          Token: NonEmptyString.T
          OrgId: NonEmptyString.T }

    [<Literal>]
    let GraphqlBaseAddress = "https://api.fly.io/graphql"

    [<Literal>]
    let RestBaseAddress = "http://_api.internal:4280"

    [<Literal>]
    let PreferredRegion = "sea"

    type MachineResponse = { id: string; name: string }

    let mkFlyIoError msg =
        [ { message = msg
            extensions = Dictionary() } ]

    type FlyIoRestClient(cfg: ClientConfig) =
        let url (s: string) = $"{cfg.BaseAddress}{s}"

        let bearerToken =
            $"Bearer {NonEmptyString.value cfg.Token}"

        let statusCodeExpectationToErr err =
            mkFlyIoError $"Received unexpected Fly.io status code ${err.actual}"

        member this.CreateMachine appName machineName config =
            http {
                POST(url $"v1/apps/{appName}/machines")
                Authorization bearerToken
                body

                jsonSerialize
                    {| name = machineName
                       config = config |}
            }
            |> Request.sendTAsync
            |> Task.map (Response.expectHttpStatusCode HttpStatusCode.OK)
            |> TaskResult.mapError statusCodeExpectationToErr
            |> TaskResult.bind (fun r ->
                Response.deserializeJsonTAsync<MachineResponse> r |> Task.map Ok)

        member this.UpdateMachine appName machineId config =
            http {
                POST(url $"v1/apps/{appName}/machines/{machineId}")
                Authorization bearerToken
                body
                jsonSerialize {| config = config |}
            }
            |> Request.sendTAsync
            |> Task.map (Response.expectHttpStatusCode HttpStatusCode.OK)
            |> TaskResult.mapError statusCodeExpectationToErr
            |> TaskResult.bind (fun r ->
                Response.deserializeJsonTAsync<MachineResponse> r |> Task.map Ok)


    let mkGraphqlClient cfg =
        let client = new HttpClient()
        client.BaseAddress <- cfg.BaseAddress

        client.DefaultRequestHeaders.Authorization <-
            AuthenticationHeaderValue(
                "Bearer",
                (NonEmptyString.value cfg.Token)
            )

        FlyIoGraphqlClient client

    let mkRestClient cfg = FlyIoRestClient cfg

    let hasError (errs: FlyIoError list) code =
        List.exists
            (fun (e: FlyIoError) ->
                e.extensions.GetValueOrDefault "code" = code)
            errs
