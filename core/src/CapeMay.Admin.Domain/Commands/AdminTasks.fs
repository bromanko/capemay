namespace CapeMay.Admin.Domain.Commands

open CapeMay.Admin.Domain
open CapeMay.Domain
open CapeMay.Domain.DataStore
open CapeMay.Admin.Domain.DataStore
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.ResultOption
open Newtonsoft.Json
open Microsoft.FSharp.Reflection

type AdminTask = | Noop

module AdminTasks =
    let private parseTaskData d = [||]

    let private parseTask (t: AsyncTask) : Result<AdminTask, _> =
        let taskName = NonEmptyString.value t.Name

        match
            FSharpType.GetUnionCases(typeof<AdminTask>)
            |> Array.filter (fun c -> c.Name = taskName)
        with
        | [| case |] ->
            FSharpValue.MakeUnion(case, parseTaskData t.Data) :?> AdminTask
            |> Ok
        | _ ->
            DomainError.UnhandledError
                $"Could not parse task of type {taskName}."
            |> Error


    let getNextTask connStr =
        use conn = mkConn connStr
        conn.Open()

        AdminTasks.getTask conn >>= (fun t -> parseTask t |> Result.map Some)
