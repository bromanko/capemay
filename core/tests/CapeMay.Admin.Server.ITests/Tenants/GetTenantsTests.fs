namespace CapeMay.Admin.Server.ITests.Tenants

open CapeMay.Admin.Server.ITests
open CapeMay.Admin.Server.ITests.Config
open CapeMay.Admin.Server.ITests.Tenants.CreateTenantTests
open Expecto
open FsHttp
open FsHttp.NewtonsoftJson

module GetTenantsTests =
    let cfg = loadConfig ()

    [<Tests>]
    let tests =
        testList
            "GetTenants"
            [ testTask "Returns tenants" {
                  let toCreate = genTenant ()
                  createTenant toCreate |> ignore

                  http { GET(serverPath cfg "tenant") }
                  |> Request.send
                  |> expectStatusCodeOK
                  |> Response.toJson
                  |> fun json ->
                      let path =
                          $"$.tenants[?(@.fqdn == '{toCreate.fqdn}')]"

                      Expect.isNotNull
                          (json.SelectToken path)
                          $"Created tenant was not returned in {json}."
              } ]
