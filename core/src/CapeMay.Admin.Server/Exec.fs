namespace CapeMay.Admin.Server

open Giraffe
open CapeMay.Admin.Server.Errors
open Microsoft.AspNetCore.Http

[<RequireQualifiedAccess>]
module Exec =
    let inline private respond status cmd : HttpHandler =
        match cmd () with
        | Ok r -> json r |> status
        | Error e -> respForDomainErr e

    let create cmd = respond Successful.created cmd

    let read cmd = respond Successful.ok cmd

    let run cmd = respond Successful.ok cmd


    let inline private respondAsync status cmd =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                match! cmd () with
                | Ok r -> return! (json r |> status) next ctx
                | Error e -> return! respForDomainErr e next ctx
            }

    let createAsync cmd = respondAsync Successful.created cmd

    let readAsync cmd = respondAsync Successful.ok cmd

    let runAsync cmd = respondAsync Successful.ok cmd
