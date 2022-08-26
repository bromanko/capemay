namespace CapeMay.Admin.Server.ITests

open Expecto
open FsHttp
open FsHttp.NewtonsoftJson

module ErrorTests =
    let cfg = Config.loadConfig ()

    [<Tests>]
    let tests =
        testList
            "Errors"
            [ testTask "Not Found" {
                  http { GET(serverPath cfg "foo") }
                  |> Request.send
                  |> expectStatusCodeNotFound
                  |> Response.toJson
                  |> fun json ->
                      Expect.equal
                          (json?``type``.ToObject<string>())
                          "NotFoundError"
                          "Response body did not contain correct error type"
              } ]
