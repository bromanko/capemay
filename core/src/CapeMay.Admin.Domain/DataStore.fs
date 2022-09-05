namespace CapeMay.Admin.Domain

open Microsoft.Data.Sqlite
open FSharpx
open CapeMay.Domain

module DataStore =
    let mkConn (connStr: string) = new SqliteConnection(connStr)

    module Tenants =
        let createTenant (t: CreateTenant) (conn: SqliteConnection) =
            // SqliteCommand.text
            //     """
            //     INSERT INTO tenants (id, fqdn) VALUES (@id, @fqdn)
            // """
            // |> SqliteCommand.parameters [ ("@id",
            //                                SqliteDbValue.Text
            //                                <| Id.toString t.Id)
            //                               ("@fqdn",
            //                                SqliteDbValue.Text
            //                                <| NonEmptyString.value t.Fqdn) ]
            // |> SqliteCommand.executeNonQuery conn
            // |> Async.StartAsTask
            // |> Task.Ignore
            let c = conn.CreateCommand()

            c.CommandText <-
                """
                INSERT INTO tenants (id, fqdn) VALUES (@id, @fqdn)
                """

            c.Parameters.AddWithValue("@id", Id.toString t.Id)
            |> ignore

            c.Parameters.AddWithValue("@fqdn", NonEmptyString.value t.Fqdn)
            |> ignore

            c.ExecuteNonQueryAsync() |> Task.Ignore
