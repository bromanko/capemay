namespace CapeMay.Admin.Server.ITests


open Expecto

module Main =
    [<EntryPoint>]
    let main argv =
        Tests.runTestsInAssembly defaultConfig argv
