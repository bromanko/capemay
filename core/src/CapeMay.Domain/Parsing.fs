namespace CapeMay.Domain

[<AutoOpen>]
module Parsing =
    open FsToolkit.ErrorHandling

    let tryParse<'T, 'TErr>
        (fn: string -> 'T option)
        (str: string)
        (msg: 'TErr)
        : Validation<'T, 'TErr> =
        match fn str with
        | None -> Validation.error msg
        | Some n -> Validation.ok n

    let tryParseNES = NonEmptyString.parse
