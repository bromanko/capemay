namespace CapeMay.Admin.TaskRunner

open FsConfig
open CapeMay.Admin.Domain

type DbConfig = { ConnectionString: string }

type FlyIoConfig =
    { [<DefaultValue(FlyIo.RestBaseAddress)>]
      RestApiBase: string
      [<DefaultValue(FlyIo.GraphqlBaseAddress)>]
      GraphqlBase: string
      Token: string }

module Config =
    [<Convention("ADMIN")>]
    [<RequireQualifiedAccess>]
    type T = { Db: DbConfig; FlyIo: FlyIoConfig }

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
