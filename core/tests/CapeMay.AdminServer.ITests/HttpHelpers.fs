namespace CapeMay.AdminServer.ITests

open System

[<AutoOpen>]
module HttpHelpers =
    let serverPath (cfg: CapeMay.AdminServer.ITests.Config) path =
        let ub = UriBuilder(cfg.Server.HttpUri)
        ub.Path <- ub.Path + path
        ub.ToString()
