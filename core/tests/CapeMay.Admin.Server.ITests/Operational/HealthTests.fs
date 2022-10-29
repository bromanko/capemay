namespace CapeMay.Admin.Server.ITests.Operational

open CapeMay.Admin.Server.ITests
open CapeMay.Admin.Server.ITests.Config
open Expecto
open FsHttp
open FsHttp.NewtonsoftJson

module HealthTests =
    let cfg = loadConfig ()

    let healthPath = serverPath cfg "_/health"

    [<Tests>]
    let tests =
        testList
            "Health Endpoint"
            [ testTask "Returns health information" {
                  http {
                      GET healthPath
                  }
                  |> Request.send
                  |> expectStatusCodeOK
                  |> Response.toJson
                  |> fun json ->
                      Expect.isNotEmpty
                          (json?status.ToObject<string>())
                          "Response did not contain status string"
              } ]

