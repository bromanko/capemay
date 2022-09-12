namespace CapeMay.Domain

open System
open System.Globalization
open System.Text

[<RequireQualifiedAccess>]
module String =
    type private SnakeCaseState =
        | Start
        | Lower
        | Upper
        | NewWord

    let toSnakeCase s =
        if String.IsNullOrEmpty s then
            s
        else
            let sb = StringBuilder()
            let mutable state = SnakeCaseState.Start

            let span = s.AsSpan()

            for i = 0 to span.Length - 1 do
                if span[i] = ' ' then
                    if state <> SnakeCaseState.Start then
                        state <- SnakeCaseState.NewWord
                    else
                        ()
                else if Char.IsUpper span[i] then
                    match state with
                    | SnakeCaseState.Upper ->
                        let hasNext = i + 1 < span.Length

                        if i > 0 && hasNext then
                            let nextChar = span[i + 1]

                            if not (Char.IsUpper nextChar) && nextChar <> '_' then
                                sb.Append '_' |> ignore
                    | SnakeCaseState.Lower
                    | SnakeCaseState.NewWord -> sb.Append '_' |> ignore
                    | SnakeCaseState.Start -> ()

                    Char.ToLower(span[i], CultureInfo.InvariantCulture)
                    |> sb.Append
                    |> ignore

                    state <- SnakeCaseState.Upper
                else if span[i] = '_' then
                    sb.Append '_' |> ignore
                else
                    if state = SnakeCaseState.NewWord then
                        sb.Append '_' |> ignore

                    sb.Append span[i] |> ignore
                    state <- SnakeCaseState.Lower

            sb.ToString()
