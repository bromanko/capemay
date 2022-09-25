namespace CapeMay.Consumer.Server


open Microsoft.Extensions.Hosting

module Main =
    [<EntryPoint>]
    let main argv =
        let compRoot =
            Config.loadConfig ()
            |> CompositionRoot.defaultRoot

        let host =
            (Host.mkHostBuilder compRoot argv).Build()

        host.Run()

        0
