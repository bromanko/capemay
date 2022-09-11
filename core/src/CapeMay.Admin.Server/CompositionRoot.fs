namespace CapeMay.Admin.Server

open System.Threading.Tasks
open CapeMay.Admin.Domain
open CapeMay.Domain

[<RequireQualifiedAccess>]
module CompositionRoot =
    type Tenants =
        { CreateAsync: CreateTenant -> Task<Result<Tenant, DomainError>> }

    type Commands = { Tenants: Tenants }

    type T =
        { Config: Config.T
          Commands: Commands }

    let defaultRoot cfg =
        { Config = cfg
          Commands =
            { Tenants =
                { CreateAsync = Commands.createTenant cfg.Db.ConnectionString } } }
