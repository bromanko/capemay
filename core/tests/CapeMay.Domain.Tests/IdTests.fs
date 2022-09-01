namespace CapeMay.Domain.Tests

open Expecto
open CapeMay.Domain

module IdTests =
    [<Tests>]
    let tests =
        testList
            "Id"
            [ test "Creates an Id from Prefix" {
                  (NonEmptyString.parse "foo").Value
                  |> Id.mkId
                  |> ignore
              }
              test "Id produces correct string with prefix" {
                  let id =
                      (NonEmptyString.parse "foo").Value |> Id.mkId

                  Expect.stringStarts
                      (Id.toString id)
                      "foo_"
                      "Id does not start with prefix"
              }
              test "Id produces correct string with correct format" {
                  let id =
                      (NonEmptyString.parse "foo").Value |> Id.mkId

                  Expect.isMatch
                      (Id.toString id)
                      "^foo_.{20}$"
                      "Id format is incorrect"
              } ]
