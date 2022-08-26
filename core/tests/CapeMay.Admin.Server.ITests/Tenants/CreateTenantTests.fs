namespace CapeMay.Admin.Server.ITests.Tenants

open System.Net
open CapeMay.Admin.Server.ITests
open CapeMay.Admin.Server.ITests.Config
open Expecto
open FsHttp
open FsHttp.NewtonsoftJson

module CreateTenantTests =
    let cfg = loadConfig ()

    let tenantPath = serverPath cfg "tenant"

    [<Tests>]
    let tests =
        testList
            "Create Tenant"
            [ testTask "Returns error with invalid json" {
                  http {
                      POST tenantPath
                      body
                      text "invalid json"
                  }
                  |> Request.send
                  |> expectStatusCodeBadRequest
                  |> Response.toJson
                  |> fun json ->
                      Expect.equal
                          (json?``type``.ToObject<string>())
                          "JsonParseError"
                          "Did not return expected error code"
              }

              testTask "Creates tenant" {
                  http {
                      POST tenantPath
                      body
                      jsonSerialize {| fqdn = "foo.bar" |}
                  }
                  |> Request.send
                  |> expectStatusCodeCreated
                  |> Response.toJson
                  |> fun json ->
                      Expect.equal
                          (json?fqdn.ToObject<string>())
                          "foo.bar"
                          "fqdn does not match"

                      Expect.isNotNull json?id "id is missing"
              } ]
