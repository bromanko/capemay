namespace CapeMay.Admin.Server.ITests

open System

[<AutoOpen>]
module HttpHelpers =
    let serverPath (cfg: CapeMay.Admin.Server.ITests.Config) path =
        let ub = UriBuilder(cfg.Server.HttpUri)
        ub.Path <- ub.Path + path
        ub.ToString()
