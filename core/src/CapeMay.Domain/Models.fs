namespace CapeMay.Domain

open System
open TryParse

module TenantId =
    type T =
        private
        | TenantId of Guid

        override this.ToString() =
            match this with
            | TenantId i -> i.ToString()

    let parse s =
        match parseGuid s with
        | Some id -> Some(TenantId id)
        | None -> None

    let create g = TenantId g

    let value (TenantId t) = t

type Tenant =
    { Id: TenantId.T
      Fqdn: NonEmptyString.T }
