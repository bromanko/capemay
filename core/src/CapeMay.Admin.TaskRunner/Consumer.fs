namespace CapeMay.Admin.TaskRunner

open System
open System.Threading
open System.Threading.Tasks
open CapeMay.Admin.Domain.Commands
open CapeMay.Domain
open Microsoft.Extensions.Hosting
open FsToolkit.ErrorHandling.Operator.ResultOption
open FsToolkit.ErrorHandling
open FSharp.Control
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Reflection

type private ConsumerMessage = | Dequeue

type Consumer
    (
        dequeueTask: DequeueFn,
        log: ILogger<Consumer>,
        ?sleepMs: TimeSpan
    ) =
    let sleepMs = defaultArg sleepMs (TimeSpan.FromSeconds 3)

    let worker = new Worker()

    let taskName (t: AdminTask) =
        match FSharpValue.GetUnionFields(t, typeof<AdminTask>) with
        | case, _ -> case.Name

    let dequeue () =
        try
            dequeueTask ()
            |> ResultOption.map (fun t ->
                log.LogInformation $"Executing {taskName t} task."
                worker.Exec t)
            |> Result.teeError (fun e ->
                match e with
                | UnhandledException ex ->
                    log.LogError(ex, "Error dequeueing task.")
                | _ -> log.LogError $"Error dequeueing task. {e}")
            |> ignore
        with ex ->
            log.LogError(ex, "Error dequeueing task.")

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
                        dequeue ()
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


type ConsumerHostedService(dequeueFn: DequeueFn, log: ILogger<Consumer>) =
    let consumer = new Consumer(dequeueFn, log)

    interface IHostedService with
        member _.StartAsync(_: CancellationToken) =
            log.LogInformation "Starting task consumer..."
            consumer.Start()
            log.LogInformation "Task consumer started."
            Task.CompletedTask

        member _.StopAsync(_: CancellationToken) =
            log.LogInformation "Stopping task consumer..."

            consumer.Stop()
            |> Async.map (fun _ -> log.LogInformation "Task consumer stopped.")
            |> Async.StartAsTask
            :> Task

    interface IDisposable with
        member _.Dispose() = (consumer :> IDisposable).Dispose()
