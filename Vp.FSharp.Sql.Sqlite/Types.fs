namespace Vp.FSharp.Sql.Sqlite

open System
open System.Data
open System.Threading.Tasks
open Microsoft.Data.Sqlite

open Vp.FSharp.Sql


/// Native Sqlite DB types.
/// See https://www.sqlite.org/datatype3.html
type SqliteDbValue =
    | Null
    | Integer of int64
    | Real    of double
    | Text    of string
    | Blob    of byte array
    | Custom  of DbType * obj

/// Sqlite Command Definition
type SqliteCommandDefinition =
    CommandDefinition<
        SqliteConnection,
        SqliteCommand,
        SqliteParameter,
        SqliteDataReader,
        SqliteTransaction,
        SqliteDbValue>

/// Sqlite Configuration
type SqliteConfiguration =
    SqlConfigurationCache<
        SqliteConnection,
        SqliteCommand>

/// Sqlite Dependencies
type SqliteDependencies =
    SqlDependencies<
        SqliteConnection,
        SqliteCommand,
        SqliteParameter,
        SqliteDataReader,
        SqliteTransaction,
        SqliteDbValue>

[<AbstractClass; Sealed>]
type internal Constants private () =

    static let beginTransactionAsync (connection: SqliteConnection) (isolationLevel: IsolationLevel) _ =
        ValueTask.FromResult(connection.BeginTransaction(isolationLevel))

    static let executeReaderAsync (command: SqliteCommand) _ =
        Task.FromResult(command.ExecuteReader())

    static let deps : SqliteDependencies =
        { CreateCommand = fun connection -> connection.CreateCommand()
          SetCommandTransaction = fun command transaction -> command.Transaction <- transaction
          BeginTransaction = fun connection -> connection.BeginTransaction
          BeginTransactionAsync = beginTransactionAsync
          ExecuteReader = fun command -> command.ExecuteReader()
          ExecuteReaderAsync = executeReaderAsync
          DbValueToParameter = Constants.DbValueToParameter }

    static member DbValueToParameter name value =
        let parameter = SqliteParameter()
        parameter.ParameterName <- name
        match value with
        | Null ->
            parameter.Value      <- DBNull.Value
        | Integer value ->
            parameter.SqliteType <- SqliteType.Integer
            parameter.Value      <- value
        | Real value ->
            parameter.SqliteType <- SqliteType.Real
            parameter.Value      <- value
        | Text value ->
            parameter.SqliteType <- SqliteType.Text
            parameter.Value      <- value
        | Blob value ->
            parameter.SqliteType <- SqliteType.Blob
            parameter.Value      <- value

        | Custom (dbType, value) ->
            parameter.DbType <- dbType
            parameter.Value  <- value

        parameter

    static member Deps = deps
