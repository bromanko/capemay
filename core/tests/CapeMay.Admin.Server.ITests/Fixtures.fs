namespace CapeMay.Admin.Server.ITests

open Bogus

[<AutoOpen>]
module Fixtures =
    let genFqdn = Faker().Internet.DomainName
