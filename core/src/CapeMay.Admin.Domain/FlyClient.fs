namespace CapeMay.Admin.Domain

open FSharp.Data.GraphQL

module FlyClient =
    type FlyProvider = GraphQLProvider<"fly_schema.json">

