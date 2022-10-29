namespace CapeMay.Admin.Server

open Giraffe
open CapeMay.Admin.Server.Errors

[<RequireQualifiedAccess>]
module Exec =
    let inline private respond status cmd =
        match cmd () with
        | Ok r -> json r |> status
        | Error e -> respForDomainErr e

    let create cmd = respond Successful.created cmd

    let read cmd = respond Successful.ok cmd

    let run cmd = respond Successful.ok cmd

