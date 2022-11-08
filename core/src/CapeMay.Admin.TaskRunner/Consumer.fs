namespace CapeMay.Admin.TaskRunner

open System
open System.Threading

type AdminTask = | Noop

module Worker =
    let mkWorker () =
        MailboxProcessor.Start(fun inbox ->
            let rec loop () =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Noop -> printfn "Received a message."

                    return! loop ()
                }

            loop ())

module Consumer =

    type T(?sleepMs: TimeSpan) =
        let sleepMs = defaultArg sleepMs (TimeSpan.FromSeconds 3)

        let worker = Worker.mkWorker ()

        let dequeue () = Noop

        member self.Loop() : unit =
            try
                dequeue () |> worker.Post
            with error ->
                printfn $"{error}"

            Thread.Sleep sleepMs
            self.Loop()
