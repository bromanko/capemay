namespace CapeMay.Server

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting
open Giraffe

module Host =
    let private webApp = choose []

    let private configureApp (app: IApplicationBuilder) = app.UseGiraffe webApp

    let private configureServices (services: IServiceCollection) =
        services.AddGiraffe() |> ignore

    let mkHostBuilder argv =
        Host
            .CreateDefaultBuilder(argv)
            .ConfigureWebHostDefaults(fun wb ->
                wb
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                |> ignore)
