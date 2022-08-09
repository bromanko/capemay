namespace CapeMay.Server
open Giraffe

module Say =
    let hello name =
        printfn "Hello %s" name
