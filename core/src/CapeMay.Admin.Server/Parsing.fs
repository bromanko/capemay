namespace CapeMay.Admin.Server

open CapeMay.Admin.Server.Errors
open Giraffe
open Microsoft.AspNetCore.Http

[<AutoOpen>]
module Parsing =
    open Chessie.ErrorHandling

    let private parseErr err next ctx =
        RequestErrors.badRequest (json (mkErrorDto err)) next ctx

    let bindAndParse<'TModelDto, 'TModelParsed when 'TModelDto: null>
        (parse: 'TModelDto -> Result<'TModelParsed, string>)
        (handler: 'TModelParsed -> HttpHandler)
        : HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                match! ctx.BindJsonAsync<'TModelDto>() with
                | null ->
                    return!
                        parseErr
                            (InputValidationError [ "Could not parse provided data." ])
                            next
                            ctx
                | model ->
                    match parse model with
                    | Result.Ok (parsed, _) -> return! handler parsed next ctx
                    | Result.Bad errs ->
                        return! parseErr (InputValidationError errs) next ctx
            }
