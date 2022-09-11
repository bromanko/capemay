namespace CapeMay.Admin.Domain

open Microsoft.Data.Sqlite
open FSharpx
open FsToolkit.ErrorHandling
open CapeMay.Domain
open Vp.FSharp.Sql.Sqlite
open CapeMay.Domain.DataStore

module DataStore =
    module Tenants =
        let createTenant (conn: SqliteConnection) (t: CreateTenant) =
            SqliteCommand.text
                """
                INSERT INTO tenants (id, fqdn) VALUES (@id, @fqdn)
            """
            |> SqliteCommand.parameters [ ("@id",
                                           SqliteDbValue.Text
                                           <| Id.toString t.Id)
                                          ("@fqdn",
                                           SqliteDbValue.Text
                                           <| NonEmptyString.value t.Fqdn) ]
            |> SqliteCommand.executeNonQuery conn
            |> Async.StartAsTask
            |> mapDataStoreErr
            |> TaskResult.map (fun _ -> { Id = t.Id; Fqdn = t.Fqdn })
