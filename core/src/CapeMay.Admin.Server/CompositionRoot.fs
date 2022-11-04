namespace CapeMay.Admin.Server

open System.Threading.Tasks
open CapeMay.Admin.Domain
open CapeMay.Domain

[<RequireQualifiedAccess>]
module CompositionRoot =
    type Tenants =
        { Create: CreateTenant -> Task<Result<Tenant, DomainError>>
          GetAll: unit -> Result<TenantList, DomainError> }

    type Db =
        { Status: unit -> Result<Commands.Db.DbStatus, DomainError>
          Deploy: unit -> Result<Sqitch.DeployResult, DomainError> }

    type Commands = { Tenants: Tenants; Db: Db }

    type T =
        { Config: Config.T; Commands: Commands }

    let defaultRoot cfg =
        { Config = cfg
          Commands =
            { Tenants =
                { Create = Commands.Tenants.createTenant cfg.Db.ConnectionString
                  GetAll =
                    fun () ->
                        Commands.Tenants.getAllTenants cfg.Db.ConnectionString }
              Db =
                { Status = fun () -> Commands.Db.status cfg.Db.ConnectionString
                  Deploy = fun () -> Commands.Db.deploy cfg.Db.ConnectionString } } }
