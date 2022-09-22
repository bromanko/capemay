namespace CapeMay.Domain

open Microsoft.Data.Sqlite
open FsToolkit.ErrorHandling
open FSharpx

module DataStore =
    let mkConn (connStr: string) = new SqliteConnection(connStr)

    let mapDataStoreErr (r: Result<'a, exn>) : Result<'a, DomainError> =
        Result.mapError
            (fun (ex: exn) ->
                match ex with
                | :? SqliteException as se when
                    String.contains "UNIQUE constraint failed" se.Message
                    ->
                    UniquenessError "Entity already exists."
                | _ -> UnhandledException ex)
            r
