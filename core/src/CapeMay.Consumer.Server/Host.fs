namespace CapeMay.Consumer.Server

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting
open Giraffe
open Microsoft.AspNetCore.Builder

module Host =
    let private webApp compRoot = choose []

    let private configureApp
        (compRoot: CompositionRoot.T)
        (app: IApplicationBuilder)
        =
        app
            // .UseGiraffeErrorHandler(Errors.errorHandler)
            .UseGiraffe(webApp compRoot)

    let private configureServices
        (_: CompositionRoot.T)
        (services: IServiceCollection)
        =
        services.AddGiraffe() |> ignore

    let private mkServerUrls (cfg: ServerConfig) = [| cfg.HttpUri.ToString() |]

    let mkHostBuilder (compRoot: CompositionRoot.T) argv =
        Host
            .CreateDefaultBuilder(argv)
            .ConfigureWebHostDefaults(fun wb ->
                wb
                    .UseUrls(mkServerUrls compRoot.Config.Server)
                    .Configure(configureApp compRoot)
                    .ConfigureServices(configureServices compRoot)
                |> ignore)
