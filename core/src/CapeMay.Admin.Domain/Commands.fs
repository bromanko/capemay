namespace CapeMay.Admin.Domain

open CapeMay.Domain.DataStore
open CapeMay.Domain

module Commands =
    let createTenant connStr t =
        use conn = mkConn connStr
        conn.Open()
        DataStore.Tenants.createTenant conn t

    let getAllTenants connStr =
        use conn = mkConn connStr
        conn.Open()
        DataStore.Tenants.getTenants conn
