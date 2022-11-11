namespace CapeMay.Admin.TaskRunner

open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

module Main =
    let mkHostBuilder (compRoot: CompositionRoot.T) =
        let hostBuilder = Host.CreateDefaultBuilder()

        hostBuilder.ConfigureServices(fun ctx (services: IServiceCollection) ->
            services.AddSingleton<DequeueFn>(compRoot.DequeueFn) |> ignore
            services.AddHostedService<ConsumerHostedService>() |> ignore)
        |> ignore

        hostBuilder

    [<EntryPoint>]
    let main _ =
        let compRoot = Config.loadConfig () |> CompositionRoot.defaultRoot
        let host = (mkHostBuilder compRoot).Build()

        host
            .Services
            .GetService<ILogger<Config.T>>()
            .LogInformation $"Configured with:\n {compRoot.Config}"

        host.Run()

        0
