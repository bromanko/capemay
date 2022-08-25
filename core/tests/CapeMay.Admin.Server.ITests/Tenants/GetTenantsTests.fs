namespace CapeMay.Admin.Server.ITests.Tenants

open CapeMay.Admin.Server.ITests
open CapeMay.Admin.Server.ITests.Config
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
