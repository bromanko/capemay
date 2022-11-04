[<RequireQualifiedAccess>]
module rec FlyIo.LaunchMachine

type InputVariables = { input: LaunchMachineInput }
type Machine = { id: string; name: string }
type LaunchMachinePayload = { machine: Machine }

type Query =
    { launchMachine: Option<LaunchMachinePayload> }
