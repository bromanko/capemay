namespace CapeMay.AdminServer.ITests

open FsHttp
open Expecto
open System.Net

[<AutoOpen>]
module Assertions =
    let expectStatusCode (resp: Response) code =
        Expect.equal resp.statusCode code "Unexpected response status code"
        resp

    let expectStatusCodeOK resp = expectStatusCode resp HttpStatusCode.OK
