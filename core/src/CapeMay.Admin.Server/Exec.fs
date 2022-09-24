namespace CapeMay.Admin.Server

open Giraffe
open CapeMay.Admin.Server.Errors

[<RequireQualifiedAccess>]
module Exec =
    let inline private respond status cmd =
        match cmd () with
        | Ok r -> json r |> status
        | Error e -> respForDomainErr e

    let read cmd = respond Successful.ok cmd
