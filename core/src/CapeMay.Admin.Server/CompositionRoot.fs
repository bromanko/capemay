namespace CapeMay.Admin.Server

[<RequireQualifiedAccess>]
module CompositionRoot =
    type T = { Config: Config.T }

    let defaultRoot cfg = { Config = cfg }
