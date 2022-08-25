namespace CapeMay.Admin.Server.ITests

open System
open FsConfig

type ServerConfig =
    { Base: string
      HttpPort: int }
    member this.HttpUri =
        Uri $"http://%s{this.Base}:%i{this.HttpPort}"

[<Convention("TEST")>]
type Config = { Server: ServerConfig }

module Config =
    let private failWithInvalidConfig error =
        match error with
        | NotFound envVarName ->
            failwithf $"Environment variable %s{envVarName} not found"
        | BadValue (envVarName, value) ->
            failwithf
                $"Environment variable %s{envVarName} has invalid value %s{value}"
        | NotSupported msg -> failwith msg

    let loadConfig () =
        match EnvConfig.Get<Config>() with
        | Ok config -> config
        | Error error -> failWithInvalidConfig error
