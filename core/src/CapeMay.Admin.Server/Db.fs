namespace CapeMay.Admin.Server

open Giraffe
open CapeMay.Admin.Server

module Db =
    let status (compRoot: CompositionRoot.T) =
        Exec.run compRoot.Commands.Db.Status

    let deploy (compRoot: CompositionRoot.T) =
        Exec.run compRoot.Commands.Db.Deploy

    [<Literal>]
    let DbPath = "/_/db"

    let routes compRoot =
        choose
            [ route DbPath
              >=> GET
              >=> warbler (fun _ -> status compRoot)
              route $"{DbPath}/deploy" >=> POST >=> deploy compRoot ]
