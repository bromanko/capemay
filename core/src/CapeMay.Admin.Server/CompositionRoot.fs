namespace CapeMay.Admin.Server

open Microsoft.Data.Sqlite
open System.Threading.Tasks
open CapeMay.Admin.Domain
open CapeMay.Admin.Domain.DataStore

[<RequireQualifiedAccess>]
module CompositionRoot =
    type DataStore =
        { MakeConnection: unit -> SqliteConnection
          CreateTenant: CreateTenant -> SqliteConnection -> Task<unit> }

    type T =
        { Config: Config.T
          DataSource: DataStore }

    let private mkDataStore (cfg: DbConfig) =
        { MakeConnection = fun () -> mkConn cfg.ConnectionString
          CreateTenant = Tenants.createTenant }


    let defaultRoot cfg =
        { Config = cfg
          DataSource = mkDataStore cfg.Db }
