namespace CapeMay.AdminServer

open CapeMay.Domain
open Giraffe
open Microsoft.AspNetCore.Http
open FSharpx

type Error = InputValidationError of string

module Errors =
    let toStringErr err =
        match err with
        | InputValidationError e -> $"Input validation failed. %s{e}"
        |> String.trim

[<AutoOpen>]
module Parsing =
    open Chessie.ErrorHandling

    let tryParseNES str msg =
        match NonEmptyString.parse str with
        | None -> fail <| Error.InputValidationError msg
        | Some n -> ok n

[<AutoOpen>]
module CommonHandlers =
    open Chessie.ErrorHandling

    let parseError err = RequestErrors.BAD_REQUEST err

    let bindAndParse<'TModelDto, 'TModelParsed>
        (parse: 'TModelDto -> Result<'TModelParsed, Error>)
        (handler: 'TModelParsed -> HttpHandler)
        : HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! model = ctx.BindJsonAsync<'TModelDto>()

                match parse model with
                | Result.Ok (parsed, _) -> return! handler parsed next ctx
                | Result.Bad errs -> return! json "error" next ctx
            }
