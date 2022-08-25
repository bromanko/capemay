namespace CapeMay.AdminServer

open FSharpx

type Error = InputValidationError of string

type ErrorDto = { Type: string; Message: string }

module Errors =
    let toStringErr err =
        match err with
        | InputValidationError e -> $"Input validation failed. %s{e}"
        |> String.trim

    let mkErrorDto t errs =
        let msg =
            List.map toStringErr errs |> String.concat "\n"

        { Type = t; Message = msg }
