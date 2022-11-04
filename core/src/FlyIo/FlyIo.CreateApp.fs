[<RequireQualifiedAccess>]
module rec FlyIo.CreateApp

type InputVariables = { input: CreateAppInput }

type App =
    { /// Unique application ID
      id: string
      /// The unique application name
      name: string }

type CreateAppPayload = { app: App }
type Query = { createApp: Option<CreateAppPayload> }
