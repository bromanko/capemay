namespace CapeMay.Domain

[<AutoOpen>]
module Parsing =
    open Chessie.ErrorHandling

    let tryParseNES str msg =
        match NonEmptyString.parse str with
        | None -> fail msg
        | Some n -> ok n
