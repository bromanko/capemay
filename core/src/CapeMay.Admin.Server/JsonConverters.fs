namespace CapeMay.Admin.Server

open System
open Newtonsoft.Json

module JsonConverters =
    type ParsableConverter<'T>(parseFn) =
        inherit JsonConverter<'T>()

        override _.WriteJson(writer: JsonWriter, value: 'T, _: JsonSerializer) =
            value.ToString() |> writer.WriteValue

        override _.ReadJson
            (
                reader: JsonReader,
                _: Type,
                _: 'T,
                _: bool,
                _: JsonSerializer
            ) : 'T =
            match ((string >> parseFn) reader.Value) with
            | Some t -> t
            | None ->
                raise (
                    JsonSerializationException(
                        $"Error converting {reader.Value} to {typeof<'T>.Name}"
                    )
                )
