namespace CapeMay.Admin.Server

open FsConfig

type ServerConfig =
    { [<DefaultValue("http://*:8080")>]
      Url: string }

type DbConfig =
    { ConnectionString: string }

module Config =
    [<Convention("ADMIN")>]
    [<RequireQualifiedAccess>]
    type T = { Server: ServerConfig; Db: DbConfig }

    let private failWithInvalidConfig error =
        match error with
        | NotFound envVarName ->
            failwithf $"Environment variable %s{envVarName} not found"
        | BadValue (envVarName, value) ->
            failwithf
                $"Environment variable %s{envVarName} has invalid value %s{value}"
        | NotSupported msg -> failwith msg

    let loadConfig () =
        match EnvConfig.Get<T>() with
        | Ok config -> config
        | Error error -> failWithInvalidConfig error
