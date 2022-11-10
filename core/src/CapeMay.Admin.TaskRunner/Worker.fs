namespace CapeMay.Admin.TaskRunner

open System

type Worker() =
    let inner =
        MailboxProcessor.Start(fun inbox ->
            let rec loop () =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Control c ->
                        match c with
                        | Stop reply ->
                            printfn "Stopping worker"
                            reply.Reply()
                            printfn "Stopped worker"
                    | Data t ->
                        match t with
                        | Noop -> printfn "Worker Received a message."

                        return! loop ()

                }

            loop ())

    member _.Exec = inner.Post

    member _.Stop() =
        inner.PostAndAsyncReply(fun reply -> Stop reply |> Control)

    interface IDisposable with
        member _.Dispose() = (inner :> IDisposable).Dispose()
