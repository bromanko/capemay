namespace CapeMay.Admin.TaskRunner

type ControlMessage = Stop of AsyncReplyChannel<unit>

type WorkerMessage<'TMsg> =
    | Control of ControlMessage
    | Task of 'TMsg

type AdminTask = | Noop
