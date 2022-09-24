namespace CapeMay.Admin.Server

open CapeMay.Admin.Domain
open CapeMay.Domain

[<RequireQualifiedAccess>]
module CompositionRoot =
    type Tenants =
        { Create: CreateTenant -> Result<Tenant, DomainError>
          GetAll: unit -> Result<TenantList, DomainError> }

    type Commands = { Tenants: Tenants }

    type T =
        { Config: Config.T
          Commands: Commands }

    let defaultRoot cfg =
        { Config = cfg
          Commands =
            { Tenants =
                { Create = Commands.createTenant cfg.Db.ConnectionString
                  GetAll =
                    fun () -> Commands.getAllTenants cfg.Db.ConnectionString } } }
