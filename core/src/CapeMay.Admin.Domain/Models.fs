namespace CapeMay.Admin.Domain


open CapeMay.Domain
open System

module TenantId =
    type T = Id.T

    [<Literal>]
    let private idPrefix = "ten"

    let private idPrefixNes =
        (NonEmptyString.parse idPrefix).Value

    let create () = Id.mkId idPrefixNes

    let parse str = Id.parse idPrefixNes str

module Fqdn =
    type T =
        private
        | Fqdn of string
        override this.ToString() =
            match this with
            | Fqdn s -> s

    let parse str =
        if String.IsNullOrEmpty str then
            None
        else
            Some(Fqdn str)

    let value (Fqdn str) = str

type CreateTenant = { Id: TenantId.T; Fqdn: Fqdn.T }

type Tenant = { Id: TenantId.T; Fqdn: Fqdn.T }
