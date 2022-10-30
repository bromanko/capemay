namespace CapeMay.Admin.Server

open System
open Newtonsoft.Json
open Microsoft.FSharp.Reflection

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

    type OptionConverter() =
        inherit JsonConverter()

        override x.CanConvert(t) =
            t.IsGenericType
            && t.GetGenericTypeDefinition() = typedefof<option<_>>

        override x.WriteJson(writer, value, serializer) =
            let value =
                if value = null then
                    null
                else
                    let _, fields =
                        FSharpValue.GetUnionFields(value, value.GetType())

                    fields[0]

            serializer.Serialize(writer, value)

        override x.ReadJson(reader, t, _, serializer) =
            let innerType = t.GetGenericArguments().[0]

            let innerType =
                if innerType.IsValueType then
                    typedefof<Nullable<_>>.MakeGenericType [| innerType |]
                else
                    innerType

            let value =
                serializer.Deserialize(reader, innerType)

            let cases = FSharpType.GetUnionCases(t)

            if value = null then
                FSharpValue.MakeUnion(cases[0], [||])
            else
                FSharpValue.MakeUnion(cases[1], [| value |])
