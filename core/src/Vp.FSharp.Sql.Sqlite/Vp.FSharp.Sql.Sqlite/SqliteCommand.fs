﻿[<RequireQualifiedAccess>]
module Vp.FSharp.Sql.Sqlite.SqliteCommand

open Vp.FSharp.Sql


/// Initialize a new command definition with the given text contained in the given string.
let text value : SqliteCommandDefinition =
    SqlCommand.text value

/// Initialize a new command definition with the given text spanning over several strings (ie. list).
let textFromList value : SqliteCommandDefinition =
    SqlCommand.textFromList value

/// Update the command definition so that when executing the command, it doesn't use any logger.
/// Be it the default one (Global, if any.) or a previously overriden one.
let noLogger commandDefinition = { commandDefinition with Logger = LoggerKind.Nothing }

/// Update the command definition so that when executing the command, it use the given overriding logger.
/// instead of the default one, aka the Global logger, if any.
let overrideLogger value commandDefinition = { commandDefinition with Logger = LoggerKind.Override value }

/// Update the command definition with the given parameters.
let parameters value (commandDefinition: SqliteCommandDefinition) : SqliteCommandDefinition =
    SqlCommand.parameters value commandDefinition

/// Update the command definition with the given cancellation token.
let cancellationToken value (commandDefinition: SqliteCommandDefinition) : SqliteCommandDefinition =
    SqlCommand.cancellationToken value commandDefinition

/// Update the command definition with the given timeout.
/// Note: kludged because SQLite doesn't support per-command timeout values.
let timeout value (commandDefinition: SqliteCommandDefinition) : SqliteCommandDefinition =
    SqlCommand.timeout value commandDefinition

/// Update the command definition and sets whether the command should be prepared or not.
let prepare value (commandDefinition: SqliteCommandDefinition) : SqliteCommandDefinition =
    SqlCommand.prepare value commandDefinition

/// Update the command definition and sets whether the command should be wrapped in the given transaction.
let transaction value (commandDefinition: SqliteCommandDefinition) : SqliteCommandDefinition =
    SqlCommand.transaction value commandDefinition

/// Execute the command and return the sets of rows as an AsyncSeq accordingly to the command definition.
/// This function runs asynchronously.
let queryAsyncSeq connection read (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.queryAsyncSeq
        connection Constants.Deps SqliteConfiguration.Snapshot read commandDefinition

/// Execute the command and return the sets of rows as an AsyncSeq accordingly to the command definition.
/// This function runs synchronously.
let querySeqSync connection read (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.querySeqSync
        connection Constants.Deps SqliteConfiguration.Snapshot read commandDefinition

/// Execute the command and return the sets of rows as a list accordingly to the command definition.
/// This function runs asynchronously.
let queryList connection read (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.queryList
        connection Constants.Deps SqliteConfiguration.Snapshot read commandDefinition

/// Execute the command and return the sets of rows as a list accordingly to the command definition.
/// This function runs synchronously.
let queryListSync connection read (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.queryListSync
        connection Constants.Deps SqliteConfiguration.Snapshot read commandDefinition

/// Execute the command and return the first set of rows as a list accordingly to the command definition.
/// This function runs asynchronously.
let querySetList connection read (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.querySetList
        connection Constants.Deps SqliteConfiguration.Snapshot read commandDefinition

/// Execute the command and return the first set of rows as a list accordingly to the command definition.
/// This function runs synchronously.
let querySetListSync connection read (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.querySetListSync
        connection Constants.Deps SqliteConfiguration.Snapshot read commandDefinition

/// Execute the command and return the 2 first sets of rows as a tuple of 2 lists accordingly to the command definition.
/// This function runs asynchronously.
let querySetList2 connection read1 read2 (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.querySetList2
        connection Constants.Deps SqliteConfiguration.Snapshot read1 read2 commandDefinition

/// Execute the command and return the 2 first sets of rows as a tuple of 2 lists accordingly to the command definition.
/// This function runs synchronously.
let querySetList2Sync connection read1 read2 (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.querySetList2Sync
        connection Constants.Deps SqliteConfiguration.Snapshot read1 read2 commandDefinition

/// Execute the command and return the 3 first sets of rows as a tuple of 3 lists accordingly to the command definition.
/// This function runs asynchronously.
let querySetList3 connection read1 read2 read3 (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.querySetList3
        connection Constants.Deps SqliteConfiguration.Snapshot read1 read2 read3 commandDefinition

/// Execute the command and return the 3 first sets of rows as a tuple of 3 lists accordingly to the command definition.
/// This function runs synchronously.
let querySetList3Sync  connection read1 read2 read3 (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.querySetList3Sync
        connection Constants.Deps SqliteConfiguration.Snapshot read1 read2 read3 commandDefinition

/// Execute the command accordingly to its definition and,
/// - return the first cell value, if it is available and of the given type.
/// - throw an exception, otherwise.
/// This function runs asynchronously.
let executeScalar<'Scalar> connection (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.executeScalar<'Scalar, _, _, _, _, _, _, _, _>
        connection Constants.Deps SqliteConfiguration.Snapshot commandDefinition

/// Execute the command accordingly to its definition and,
/// - return the first cell value, if it is available and of the given type.
/// - throw an exception, otherwise.
/// This function runs synchronously.
let executeScalarSync<'Scalar> connection (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.executeScalarSync<'Scalar, _, _, _, _, _, _, _, _>
        connection Constants.Deps SqliteConfiguration.Snapshot commandDefinition

/// Execute the command accordingly to its definition and,
/// - return Some, if the first cell is available and of the given type.
/// - return None, if first cell is DBNull.
/// - throw an exception, otherwise.
/// This function runs asynchronously.
let executeScalarOrNone<'Scalar> connection (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.executeScalarOrNone<'Scalar, _, _, _, _, _, _, _, _>
        connection Constants.Deps SqliteConfiguration.Snapshot commandDefinition

/// Execute the command accordingly to its definition and,
/// - return Some, if the first cell is available and of the given type.
/// - return None, if first cell is DBNull.
/// - throw an exception, otherwise.
/// This function runs synchronously.
let executeScalarOrNoneSync<'Scalar> connection (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.executeScalarOrNoneSync<'Scalar, _, _, _, _, _, _, _, _>
        connection Constants.Deps SqliteConfiguration.Snapshot commandDefinition

/// Execute the command accordingly to its definition and, return the number of rows affected.
/// This function runs asynchronously.
let executeNonQuery connection (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.executeNonQuery
        connection Constants.Deps SqliteConfiguration.Snapshot commandDefinition

/// Execute the command accordingly to its definition and, return the number of rows affected.
/// This function runs synchronously.
let executeNonQuerySync connection (commandDefinition: SqliteCommandDefinition) =
    SqlCommand.executeNonQuerySync
        connection Constants.Deps SqliteConfiguration.Snapshot commandDefinition
