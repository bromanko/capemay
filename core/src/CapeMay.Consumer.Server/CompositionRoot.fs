namespace CapeMay.Consumer.Server

[<RequireQualifiedAccess>]
module CompositionRoot =
    type T = { Config: Config.T }

    let defaultRoot cfg = { Config = cfg }
