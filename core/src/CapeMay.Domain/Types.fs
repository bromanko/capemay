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


[<RequireQualifiedAccess>]
module Id =
    open Nanoid
    open FSharpx
    open FSharpx.Option

    [<Literal>]
    let private separator = '_'

    [<Struct>]
    type T =
        private
        | Id of string * string

        override this.ToString() =
            match this with
            | Id (p, u) -> $"{p}{separator}{u}"

    let private mkUnique () =
        Nanoid.Generate(
            "0123456789abcdefghjkmnpqrstvwxyzABCDEFGHJKMNPQRSTVWXYZ",
            20
        )

    let private mkId' (prefix: NonEmptyString.T) (unique: NonEmptyString.T) =
        Id((NonEmptyString.value prefix), (NonEmptyString.value unique))

    let mkId (prefix: NonEmptyString.T) =
        Id(NonEmptyString.value prefix, mkUnique ())

    let private splitId id =
        match String.splitChar [| separator |] id with
        | [| prefix; unique |] -> Some(prefix, unique)
        | _ -> None

    let parsePrefix prefix p =
        NonEmptyString.parse p
        >>= someIf (fun p -> p = prefix)

    let parse prefix id =
        splitId id
        >>= fun (p, u) ->
                mkId'
                <!> parsePrefix prefix p
                <*> NonEmptyString.parse u

    let toString id = id.ToString()
