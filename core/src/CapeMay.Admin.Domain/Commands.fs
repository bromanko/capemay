namespace CapeMay.Admin.Domain

open CapeMay.Domain.DataStore
open CapeMay.Domain

module Commands =
    let createTenant connStr t =
        task {
            use conn = mkConn connStr
            do! conn.OpenAsync()
            return! DataStore.Tenants.createTenant conn t
        }
