namespace CapeMay.Admin.Server

open CapeMay.Admin.Domain
open CapeMay.Domain

[<RequireQualifiedAccess>]
module CompositionRoot =
    type Tenants =
        { Create: CreateTenant -> Result<Tenant, DomainError>
          GetAll: unit -> Result<TenantList, DomainError> }

    type Db =
        { Status : unit -> Result<Sqitch.SqitchStatus, DomainError> }

    type Commands = { Tenants: Tenants
                      Db: Db }

    type T =
        { Config: Config.T
          Commands: Commands }

    let defaultRoot cfg =
        { Config = cfg
          Commands =
            { Tenants =
                { Create = Commands.Tenants.createTenant cfg.Db.ConnectionString
                  GetAll = fun () -> Commands.Tenants.getAllTenants cfg.Db.ConnectionString }
              Db = { Status = fun() -> Commands.Db.status cfg.Db.ConnectionString } } }
