namespace CapeMay.Admin.Server

open FSharpx
open Giraffe
open Microsoft.Extensions.Logging
open System
open CapeMay.Domain

type Error =
    | InputValidationError of string list
    | JsonParseError of string
    | UnhandledError of string
    | NotFoundError

module Errors =
    open Microsoft.FSharp.Reflection

    let toStringErr err =
        match err with
        | InputValidationError errs ->
            errs
            |> String.concat " "
            |> sprintf "Input validation failed. %s"
        | JsonParseError s -> $"Could not parse string as JSON. {s}"
        | UnhandledError s -> $"An unexpected error occurred. {s}"
        | NotFoundError -> "The path specified does not exist."
        |> String.trim

    let private getUnionCaseName (x: 'a) =
        match FSharpValue.GetUnionFields(x, typeof<'a>) with
        | case, _ -> case.Name |> String.toSnakeCase

    let mkErrorDto e =
        {| Type = getUnionCaseName e
           Message = toStringErr e |}

    let private respForEx (ex: exn) =
        let errType, body =
            match ex with
            | :? Newtonsoft.Json.JsonReaderException as ex ->
                RequestErrors.badRequest, mkErrorDto (JsonParseError ex.Message)
            | _ ->
                ServerErrors.internalError,
                mkErrorDto (UnhandledError ex.Message)

        errType (json body)

    let errorHandler (ex: Exception) (logger: ILogger) =
        logger.LogError(
            EventId(),
            ex,
            "An unhandled exception has occurred while executing the request."
        )

        clearResponse >=> respForEx ex

    let respForDomainErr e =
        match e with
        | UniquenessError e ->
            mkErrorDto (InputValidationError [ e ])
            |> json
            |> RequestErrors.badRequest
        | DomainError.UnhandledError e ->
            mkErrorDto (UnhandledError e)
            |> json
            |> ServerErrors.internalError
        | UnhandledException e -> raise e

    let notFoundHandler: HttpHandler =
        RequestErrors.notFound (json (mkErrorDto NotFoundError))
