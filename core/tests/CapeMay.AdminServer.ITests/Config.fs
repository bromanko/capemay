namespace CapeMay.AdminServer.ITests

open System

type ServerConfig =
    { Base: string
      HttpPort: int }
    member this.HttpUri =
        Uri $"http://%s{this.Base}:%i{this.HttpPort}"

type Config = { Server: ServerConfig }

