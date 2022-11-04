namespace CapeMay.Admin.Domain

open System
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

    let mkClient cfg =
        let client = new HttpClient()
        client.BaseAddress <- cfg.BaseAddress

        client.DefaultRequestHeaders.Authorization <-
            AuthenticationHeaderValue(
                "Bearer",
                (NonEmptyString.value cfg.Token)
            )

        FlyIoGraphqlClient client


