namespace CapeMay.AdminServer



open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting
open Giraffe
open Microsoft.AspNetCore.Builder
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

module Host =
    let private webApp =
        choose [
            Tenants.routes()
            RequestErrors.NOT_FOUND "Not Found"
        ]

    let private configureApp (app: IApplicationBuilder) = app.UseGiraffe webApp

    let private configureJsonSerialization (services: IServiceCollection) =
      let s = JsonSerializerSettings()
      s.ContractResolver <- DefaultContractResolver(NamingStrategy = SnakeCaseNamingStrategy())

      s.Converters.Add(JsonConverters.TenantIdConverter())
      s.Converters.Add(JsonConverters.NonEmptyStringConverter())

      services.AddSingleton<Json.ISerializer>(NewtonsoftJson.Serializer(s))

    let private configureServices (services: IServiceCollection) =
        services.AddGiraffe()
        |> configureJsonSerialization
        |> ignore

    let mkHostBuilder argv =
        Host
            .CreateDefaultBuilder(argv)
            .ConfigureWebHostDefaults(fun wb ->
                wb
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                |> ignore)
