namespace CapeMay.Admin.Domain

open Microsoft.Data.Sqlite
open FSharpx
open FsToolkit.ErrorHandling
open CapeMay.Domain
open Vp.FSharp.Sql
open Vp.FSharp.Sql.Sqlite
open CapeMay.Domain.DataStore

module DataStore =
    let mkNes (fieldName: string) (reader: SqlRecordReader<_>) =
        (reader.Value<string> fieldName |> NonEmptyString.parse).Value

    module Tenants =
        let createTenant (conn: SqliteConnection) (t: CreateTenant) =
            SqliteCommand.text
                """
                INSERT INTO tenants (id, fqdn) VALUES (@id, @fqdn)
            """
            |> SqliteCommand.parameters
                [ ("@id", SqliteDbValue.Text <| Id.toString t.Id)
                  ("@fqdn", SqliteDbValue.Text <| Fqdn.value t.Fqdn) ]
            |> Result.protect (SqliteCommand.executeNonQuerySync conn)
            |> mapDataStoreErr
            |> Result.map (fun _ -> { Id = t.Id; Fqdn = t.Fqdn })

        let mkTenantId (read: SqlRecordReader<_>) =
            (read.Value<string> "id" |> TenantId.parse).Value

        let mkFqdn (read: SqlRecordReader<_>) =
            (read.Value<string> "fqdn" |> Fqdn.parse).Value

        let getTenants (conn: SqliteConnection) =
            let readRow _ _ (read: SqlRecordReader<_>) =
                { Id = mkTenantId read
                  Fqdn = mkFqdn read }

            SqliteCommand.text
                """
                SELECT id, fqdn FROM tenants LIMIT 1000
                """
            |> Result.protect (SqliteCommand.queryListSync conn readRow)
            |> mapDataStoreErr

    module AdminTasks =
        let private readRow _ _ (read: SqlRecordReader<_>) =
            { Id = read.Value<int64> "id"
              Name = mkNes "name" read
              Data = mkNes "data" read }

        let getTask (conn: SqliteConnection) =
            SqliteCommand.text
                """
                SELECT id, name, data FROM tasks ORDER BY id LIMIT 1;
                """
            |> Result.protect (SqliteCommand.queryListSync conn readRow)
            |> mapDataStoreErr
            |> Result.map (fun tl ->
                match tl with
                | [] -> None
                | hd :: _ -> Some hd)
