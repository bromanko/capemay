namespace CapeMay.Admin.TaskRunner

open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection

module Main =
    let mkHostBuilder _ =
    let hostBuilder = Host.CreateDefaultBuilder()

    hostBuilder.ConfigureServices(fun ctx (services: IServiceCollection) ->
        services.AddHostedService<ConsumerHostedService>() |> ignore)
    |> ignore

    hostBuilder
    [<EntryPoint>]
    let main _ =
        let compRoot = Config.loadConfig () |> CompositionRoot.defaultRoot
        let host = (mkHostBuilder compRoot).Build()
        host.Run()

        0
