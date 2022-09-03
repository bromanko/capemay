namespace CapeMay.Admin.Server

open System
open FsConfig

module Config =
    type ServerConfig =
        { [<DefaultValue("localhost")>]
          Base: string
          HttpPort: int }
        member this.HttpUri =
            Uri $"http://%s{this.Base}:%i{this.HttpPort}"

    [<Convention("ADMIN")>]
    [<RequireQualifiedAccess>]
    type T = { Server: ServerConfig }

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
