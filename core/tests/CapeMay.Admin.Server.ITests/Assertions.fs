namespace CapeMay.Admin.Server.ITests

open FsHttp
open Expecto
open System.Net

[<AutoOpen>]
module Assertions =
    let expectStatusCode (resp: Response) code =
        Expect.equal resp.statusCode code "Unexpected response status code"
        resp

    let expectStatusCodeOK resp = expectStatusCode resp HttpStatusCode.OK

    let expectStatusCodeCreated resp = expectStatusCode resp HttpStatusCode.Created

    let expectStatusCodeNotFound resp = expectStatusCode resp HttpStatusCode.NotFound

    let expectStatusCodeBadRequest resp = expectStatusCode resp HttpStatusCode.BadRequest
