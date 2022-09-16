namespace CapeMay.Admin.Server.ITests.Tenants

open CapeMay.Admin.Server.ITests
open CapeMay.Admin.Server.ITests.Config
open Expecto
open FsHttp
open FsHttp.NewtonsoftJson

module CreateTenantTests =
    let cfg = loadConfig ()

    let tenantPath = serverPath cfg "tenant"

    let createTenant t =
        http {
            POST tenantPath
            body
            jsonSerialize t
        }
        |> Request.send

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
                          "json_parse_error"
                          "Did not return expected error code"
              }

              testTask "Validates fqdn" {
                  let fqdn = "foo"

                  {| (genTenant ()) with fqdn = fqdn |}
                  |> createTenant
                  |> expectStatusCodeBadRequest
                  |> Response.toJson
                  |> fun json ->
                      Expect.stringStarts
                          (json?message.ToObject<string>())
                          "Input validation failed. FQDN is invalid."
                          "Error message was unexpected"
              }

              testTask "Creates tenant" {
                  let toCreate = genTenant ()

                  createTenant toCreate
                  |> expectStatusCodeCreated
                  |> Response.toJson
                  |> fun json ->
                      Expect.equal
                          (json?fqdn.ToObject<string>())
                          toCreate.fqdn
                          "fqdn does not match"

                      Expect.isNotNull json?id "id is missing"
              }

              testTask "Fails on a duplicate tenant" {
                  let tenant = genTenant ()
                  createTenant tenant |> ignore

                  createTenant tenant
                  |> expectStatusCodeBadRequest
                  |> Response.toJson
                  |> fun json ->
                      Expect.equal
                          (json?``type``.ToObject<string>())
                          "input_validation_error"
                          "Did not return expected error code"
              } ]
