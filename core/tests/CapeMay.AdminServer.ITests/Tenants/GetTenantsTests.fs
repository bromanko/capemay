namespace CapeMay.AdminServer.ITests.Tenants

open System
open CapeMay.AdminServer.ITests.Config
open CapeMay.AdminServer.ITests.Assertions
open Expecto
open FsHttp
open FsHttp.NewtonsoftJson
open Newtonsoft.Json.Linq

module GetTenantsTests =
    let cfg = loadConfig ()

    let serverPath (cfg: CapeMay.AdminServer.ITests.Config) path =
        let ub = UriBuilder(cfg.Server.HttpUri)
        ub.Path <- ub.Path + path
        ub.ToString()

    [<Tests>]
    let tests =
        testList "GetTenants" [ testTask "Returns tenants" {
            http {
                GET (serverPath cfg "tenant")
            }
            |> Request.send
            |> expectStatusCodeOK
            |> Response.toJson
            |> fun json ->
                Expect.equal(json?tenants.[0]?fqdn.ToObject<string>()) "test" "First fqdn does not match"
        } ]
