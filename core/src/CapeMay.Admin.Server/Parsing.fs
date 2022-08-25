namespace CapeMay.Admin.Server

open CapeMay.Domain
open CapeMay.Admin.Server.Errors
open Giraffe
open Microsoft.AspNetCore.Http

[<AutoOpen>]
module Parsing =
    open Chessie.ErrorHandling

    let tryParseNES str msg =
        match NonEmptyString.parse str with
        | None -> fail msg
        | Some n -> ok n

    let parseError err = RequestErrors.BAD_REQUEST err

    let bindAndParse<'TModelDto, 'TModelParsed>
        (parse: 'TModelDto -> Result<'TModelParsed, string>)
        (handler: 'TModelParsed -> HttpHandler)
        : HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! model = ctx.BindJsonAsync<'TModelDto>()

                match parse model with
                | Result.Ok (parsed, _) -> return! handler parsed next ctx
                | Result.Bad errs ->
                    return!
                        json (mkErrorDto (InputValidationError errs)) next ctx
            }
