namespace CapeMay.Admin.Domain


open CapeMay.Domain

module TenantId =
    type T = Id.T

    [<Literal>]
    let private idPrefix = "ten_"

    let private idPrefixNes =
        (NonEmptyString.parse idPrefix).Value

    let create () = Id.mkId idPrefixNes

    let parse str = Id.parse idPrefixNes str

type CreateTenant =
    { Id: TenantId.T
      Fqdn: NonEmptyString.T }

type Tenant =
    { Id: TenantId.T
      Fqdn: NonEmptyString.T }
