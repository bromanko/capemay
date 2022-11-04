[<RequireQualifiedAccess>]
module rec FlyIo.CreateVolume

type InputVariables = { input: CreateVolumeInput }
type Volume = { id: string; name: string }
type CreateVolumePayload = { volume: Volume }

type Query =
    { createVolume: Option<CreateVolumePayload> }
