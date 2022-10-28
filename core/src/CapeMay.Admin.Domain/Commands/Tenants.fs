namespace CapeMay.Admin.Domain.Commands

open CapeMay.Admin.Domain
open CapeMay.Domain.DataStore
open CapeMay.Domain
open FSharpx
open FsToolkit.ErrorHandling

module Tenants =
    let createTenant connStr t =
        use conn = mkConn connStr
        conn.Open()
        DataStore.Tenants.createTenant conn t

    let getAllTenants connStr =
        use conn = mkConn connStr
        conn.Open()

        DataStore.Tenants.getTenants conn
        |> Result.map (fun tl -> { Tenants = tl })
