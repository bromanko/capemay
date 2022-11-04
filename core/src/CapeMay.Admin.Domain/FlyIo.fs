namespace CapeMay.Admin.Domain

open System
open System.Collections.Generic
open System.Net.Http
open System.Net.Http.Headers
open CapeMay.Domain
open FlyIo
open System.Threading.Tasks
open FsToolkit.ErrorHandling

module FlyIo =
    [<Literal>]
    let GraphqlBaseAddress = "https://api.fly.io/graphql"

    [<Literal>]
    let PreferredRegion = "sea"

    type ClientConfig =
        { BaseAddress: Uri
          Token: NonEmptyString.T
          OrgId: NonEmptyString.T }

    let mkGraphqlClient cfg =
        let client = new HttpClient()
        client.BaseAddress <- cfg.BaseAddress

        client.DefaultRequestHeaders.Authorization <-
            AuthenticationHeaderValue(
                "Bearer",
                (NonEmptyString.value cfg.Token)
            )

        FlyIoGraphqlClient client

    let hasError (errs: FlyIoError list) code =
        List.exists
            (fun (e: FlyIoError) ->
                e.extensions.GetValueOrDefault "code" = code)
            errs

    let mkFlyIoError msg =
        [ { message = msg
            extensions = Dictionary() } ]
