namespace CapeMay.Admin.Server.ITests.Operational

open CapeMay.Admin.Server.ITests
open CapeMay.Admin.Server.ITests.Config
open Expecto
open FsHttp
open FsHttp.NewtonsoftJson

module DbTests =
    let cfg = loadConfig ()

    let dbPath = serverPath cfg "_/db"

    [<Tests>]
    let tests =
        testList
            "Db Endpoint"
            [ testTask "Returns last deployment information" {
                  http {
                      GET dbPath
                  }
                  |> Request.send
                  |> expectStatusCodeOK
                  |> Response.toJson
                  |> fun json ->
                      Expect.isNotNull
                          (json?last_deploy)
                          "Response did not contain last_deploy key"
              } ]

