namespace CapeMay.Admin.TaskRunner

type DequeueFn<'t> = unit -> 't

[<RequireQualifiedAccess>]
module CompositionRoot =
    type T =
        { Config: Config.T
          DequeueFn: DequeueFn<AdminTask> }

    let private dequeueFromDb connStr = (fun _ -> Noop)

    let defaultRoot cfg =
        { Config = cfg
          DequeueFn = dequeueFromDb cfg.Db.ConnectionString }
