namespace CapeMay.Admin.Server

open Giraffe
open CapeMay.Admin.Server

module Db =
    let status (compRoot: CompositionRoot.T) =
        Exec.run compRoot.Commands.Db.Status

    [<Literal>]
    let DbPath = "/db"

    let routes compRoot =
        choose
            [ route DbPath
              >=> GET
              >=> warbler (fun _ -> status compRoot) ]
