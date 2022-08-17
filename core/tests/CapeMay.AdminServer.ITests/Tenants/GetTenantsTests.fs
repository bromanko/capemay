namespace CapeMay.AdminServer.ITests.Tenants

open System.Net
open CapeMay.AdminServer.ITests.Config
open Expecto
open FsHttp

module GetTenantsTests =
    let cfg = loadConfig ()

    [<Tests>]
    let tests =
        testList "GetTenants" [ testTask "Returns tenants" {
            http {
                GET $"{cfg.Server.HttpUri.ToString()}/tenant/"
            }
            |> Request.send
            |> assertStatusSuccess
            |> ignore
        } ]
