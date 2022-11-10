namespace CapeMay.Admin.TaskRunner

open System
open System.Threading
open Microsoft.Extensions.Hosting

type private ConsumerMessage = | Dequeue


type Consumer(dequeueTask: DequeueFn<_>, ?sleepMs: TimeSpan) =
    let sleepMs = defaultArg sleepMs (TimeSpan.FromSeconds 3)


    let worker = new Worker()

    let dequeue () = Data Noop

    let consumer =
        MailboxProcessor.Start(fun inbox ->
            let rec loop () =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Control (Stop reply) ->
                        do! worker.Stop()
                        reply.Reply()
                    | Data Dequeue ->
                        try
                            dequeueTask () |> Data |> worker.Exec
                        with error ->
                            printfn $"{error}"

                        return! loop ()
                }

            loop ())

    let timer =
        new Timer(
            TimerCallback(fun _ -> Data Dequeue |> consumer.Post),
            null,
            Timeout.Infinite,
            Timeout.Infinite
        )

    member _.Start() =
        timer.Change(TimeSpan.Zero, sleepMs) |> ignore

    member _.Stop() =
        timer.Change(Timeout.Infinite, Timeout.Infinite) |> ignore
        consumer.PostAndAsyncReply(fun reply -> Stop reply |> Control)

    interface IDisposable with
        member this.Dispose() =
            (timer :> IDisposable).Dispose()

            (consumer :> IDisposable).Dispose()
            (worker :> IDisposable).Dispose()


type ConsumerHostedService(dequeueFn: DequeueFn<_>) =
    let consumer = new Consumer(dequeueFn)

    interface IHostedService with
        member _.StartAsync(_: CancellationToken) =
            consumer.Start()
            System.Threading.Tasks.Task.CompletedTask

        member _.StopAsync(_: CancellationToken) =
            consumer.Stop() |> Async.StartAsTask :> System.Threading.Tasks.Task

    interface IDisposable with
        member _.Dispose() = (consumer :> IDisposable).Dispose()
