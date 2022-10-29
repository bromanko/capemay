namespace CapeMay.Admin.Domain.Commands

open System.IO
open CapeMay.Admin.Domain
open CapeMay.Domain
open FSharpx
open FsToolkit.ErrorHandling
open Microsoft.Data.Sqlite
open FsToolkit.ErrorHandling.Operator.Result

module Db =
    [<Literal>]
    let target = "admin"

    type DbStatus =
    { LastDeploy: Sqitch.SqitchStatus option }
    let private parseDbPath connStr =
        let builder =
            SqliteConnectionStringBuilder connStr

        builder.DataSource
        |> NonEmptyString.parse
        |> Result.ofOptionF (fun _ ->
            DomainError.UnhandledError
                $"Could not parse path from database connection string: {connStr}.")
        |> Result.map (fun ds ->
            let dir =
                NonEmptyString.value ds
                |> Path.GetFullPath
                |> Path.GetDirectoryName
                |> sprintf "%s/../" // Assuming one directory up from the datasource path
                |> Path.GetFullPath
                |> NonEmptyString.parse

            dir.Value)

    let status connStr =
        parseDbPath connStr
        >>= (fun dbPath ->
            Sqitch.status dbPath (NonEmptyString.parse target).Value)
        >>= (fun ss -> Ok { LastDeploy = ss })
