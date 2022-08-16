namespace CapeMay.AdminServer.ITests.Tenants

open CapeMay.AdminServer.ITests.Config
open Expecto

module GetTenantsTests =
    let cfg = loadConfig ()

    [<Tests>]
    let tests =
        testList "GetTenants" [ testTask "foo" { () } ]
