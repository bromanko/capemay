namespace CapeMay.Domain

open System

module TryParse =
    let tryParseWith (tryParseFunc: string -> bool * _) =
        tryParseFunc
        >> function
            | true, v -> Some v
            | false, _ -> None

    let parseGuid = tryParseWith Guid.TryParse

module NonEmptyString =
    type T =
        private
        | NonEmptyString of string
        override this.ToString() =
            match this with
            | NonEmptyString s -> s.ToString()

    let parse str =
        if String.IsNullOrEmpty(str) then
            None
        else
            Some(NonEmptyString str)

    let value (NonEmptyString str) = str
