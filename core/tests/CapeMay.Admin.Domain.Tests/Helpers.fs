namespace CapeMay.Admin.Domain.Tests

open Expecto

[<AutoOpen>]
module Helpers =
    let testCases<'TParam> fmtStr (pTest: 'TParam -> unit) =
        List.map
        <| fun p -> testCase <| sprintf fmtStr p <| fun () -> pTest p
