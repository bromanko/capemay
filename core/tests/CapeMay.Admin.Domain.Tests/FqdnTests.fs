namespace CapeMay.Admin.Domain.Tests

open Expecto
open CapeMay.Admin.Domain
open CapeMay.Admin.Domain.FlyClient

module FqdnTests =
    [<Tests>]
    let tests =
        testList
            "Fqdn"
            [ testList
                  "Parsing invalid fqdn"
                  ([ ""; "foo"; "foo."; "."; "foo.com." ]
                   |> testCases "%s is valid" (fun f ->
                       Expect.isNone (Fqdn.parse f) "Parsing was successful"))
              testList
                  "Parsing valid fqdn"
                  ([ "foo.com"; "foo.bar.com" ]
                   |> testCases "%s is not valid" (fun f ->
                       Expect.isSome (Fqdn.parse f) "Parsing was not successful"))

              ftestList "GraphQl" [ testTask "foo" {
                  let r =
                      mkRuntimeCxt
                      |> appExists "foo"
                  printfn $"{r}"
              } ] ]
