namespace CapeMay.Admin.TaskRunner

open CapeMay.Admin.Domain.Commands
open CapeMay.Domain

type DequeueFn = unit -> Result<AdminTask option, DomainError>

[<RequireQualifiedAccess>]
module CompositionRoot =
    type T =
        { Config: Config.T
          DequeueFn: DequeueFn }

    let defaultRoot cfg =
        { Config = cfg
          DequeueFn = fun () -> AdminTasks.getNextTask cfg.Db.ConnectionString }
