namespace CapeMay.Admin.Server

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting
open Giraffe
open Microsoft.AspNetCore.Builder
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

module Host =
    let private webApp =
        choose [ Tenants.routes ()
                 Errors.notFoundHandler ]

    let private configureApp
        (_: CompositionRoot.T)
        (app: IApplicationBuilder)
        =
        app
            .UseGiraffeErrorHandler(Errors.errorHandler)
            .UseGiraffe webApp

    let private configureJsonSerialization (services: IServiceCollection) =
        let s = JsonSerializerSettings()

        s.ContractResolver <-
            DefaultContractResolver(NamingStrategy = SnakeCaseNamingStrategy())

        s.Converters.Add(JsonConverters.TenantIdConverter())
        s.Converters.Add(JsonConverters.NonEmptyStringConverter())

        services.AddSingleton<Json.ISerializer>(NewtonsoftJson.Serializer(s))

    let private configureServices
        (_: CompositionRoot.T)
        (services: IServiceCollection)
        =
        services.AddGiraffe()
        |> configureJsonSerialization
        |> ignore

    let private mkServerUrls (cfg: Config.ServerConfig) =
        [| cfg.HttpUri.ToString() |]

    let mkHostBuilder (compRoot: CompositionRoot.T) argv =
        Host
            .CreateDefaultBuilder(argv)
            .ConfigureWebHostDefaults(fun wb ->
                wb
                    .UseUrls(mkServerUrls compRoot.Config.Server)
                    .Configure(configureApp compRoot)
                    .ConfigureServices(configureServices compRoot)
                |> ignore)
