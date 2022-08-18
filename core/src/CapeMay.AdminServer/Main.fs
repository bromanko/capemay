namespace CapeMay.AdminServer


open Microsoft.Extensions.Hosting

module Main =
    [<EntryPoint>]
    let main argv =
        let host = (Host.mkHostBuilder argv).Build()

        host.Run()
        0
