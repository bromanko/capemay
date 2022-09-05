namespace CapeMay.Domain

[<AutoOpen>]
module Parsing =
    open FsToolkit.ErrorHandling

    let tryParseNES str msg =
        match NonEmptyString.parse str with
        | None -> Validation.error msg
        | Some n -> Validation.ok n
