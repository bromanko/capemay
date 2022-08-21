namespace CapeMay.AdminServer.ITests.Tenants

open CapeMay.AdminServer.ITests
open CapeMay.AdminServer.ITests.Config
open Expecto
open FsHttp
open FsHttp.NewtonsoftJson

module GetTenantsTests =
    let cfg = loadConfig ()

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
