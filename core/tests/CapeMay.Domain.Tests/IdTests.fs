namespace CapeMay.Domain.Tests

open Expecto
open CapeMay.Domain

module IdTests =
    let prefix =
        (NonEmptyString.parse "foo").Value

    let testCases<'TParam> fmtStr (pTest: 'TParam -> unit) =
        List.map
        <| fun p -> testCase <| sprintf fmtStr p <| fun () -> pTest p

    [<Tests>]
    let tests =
        testList
            "Id"
            [ test "Creates an Id from Prefix" { prefix |> Id.mkId |> ignore }
              test "Id produces correct string with prefix" {
                  let id = Id.mkId prefix

                  Expect.stringStarts
                      (Id.toString id)
                      "foo_"
                      "Id does not start with prefix"
              }
              test "Id produces correct string with correct format" {
                  let id = Id.mkId prefix

                  Expect.isMatch
                      (Id.toString id)
                      "^foo_.{20}$"
                      "Id format is incorrect"
              }
              testList
                  "Parsing"
                  ([ ""
                     "no"
                     "foo"
                     "no_"
                     "_foo_"
                     "foo00" ]
                   |> testCases "%s is not valid" (fun id ->
                       Expect.isNone
                           (Id.parse prefix id)
                           "Parsing was successful")) ]
