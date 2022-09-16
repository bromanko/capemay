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
    open System.Text.RegularExpressions

    type T =
        private
        | Fqdn of string
        override this.ToString() =
            match this with
            | Fqdn s -> s

    let private fqdnRegex =
        Regex(
            "(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{0,62}[a-zA-Z0-9]\.)+[a-zA-Z]{2,63}$)"
        )

    let parse str =
        match fqdnRegex.IsMatch str with
        | true ->
            printf "All good"
            Some <| Fqdn str
        | false -> None

    let value (Fqdn str) = str

type CreateTenant = { Id: TenantId.T; Fqdn: Fqdn.T }

type Tenant = { Id: TenantId.T; Fqdn: Fqdn.T }
