namespace CapeMay.Admin.Server

open System
open CapeMay.Domain
open Newtonsoft.Json

module JsonConverters =
    type TenantIdConverter() =
        inherit JsonConverter<TenantId.T>()

        override _.WriteJson
            (
                writer: JsonWriter,
                value: TenantId.T,
                _: JsonSerializer
            ) =
            value.ToString() |> writer.WriteValue

        override _.ReadJson
            (
                reader: JsonReader,
                _: Type,
                _: TenantId.T,
                _: bool,
                _: JsonSerializer
            ) =
            match ((string >> TenantId.parse) reader.Value) with
            | Some t -> t
            | None ->
                raise (
                    JsonSerializationException(
                        $"Error converting {reader.Value} to TenantId.T"
                    )
                )

    type NonEmptyStringConverter() =
        inherit JsonConverter<NonEmptyString.T>()

        override _.WriteJson
            (
                writer: JsonWriter,
                value: NonEmptyString.T,
                _: JsonSerializer
            ) =
            value.ToString() |> writer.WriteValue

        override _.ReadJson
            (
                reader: JsonReader,
                _: Type,
                _: NonEmptyString.T,
                _: bool,
                _: JsonSerializer
            ) =
            match ((string >> NonEmptyString.parse) reader.Value) with
            | Some t -> t
            | None ->
                raise (
                    JsonSerializationException(
                        $"Error converting {reader.Value} to NonEmptyString.T"
                    )
                )
