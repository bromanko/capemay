namespace CapeMay.Domain

open Microsoft.Data.Sqlite
open System.Threading.Tasks
open FsToolkit.ErrorHandling
open FSharpx.String
open FSharpx

module DataStore =
    let mkConn (connStr: string) = new SqliteConnection(connStr)

    let private choiceToResult (c: Choice<'a, 'b>) : Result<'a, 'b> =
        match c with
        | Choice1Of2 c -> Ok c
        | Choice2Of2 c -> Error c

    let mapDataStoreErr (t: Task<'a>) : Task<Result<'a, DomainError>> =
        t
        |> Task.Catch
        |> Task.map choiceToResult
        |> TaskResult.mapError (fun ex ->
            match ex with
            | :? SqliteException as se when
                contains "UNIQUE constraint failed" se.Message
                ->
                UniquenessError "Entity already exists."
            | _ -> UnhandledException ex)
