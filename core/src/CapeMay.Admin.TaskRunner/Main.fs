namespace CapeMay.Admin.TaskRunner

module Main =
    [<EntryPoint>]
    let main argv =
        let compRoot = Config.loadConfig () |> CompositionRoot.defaultRoot
        let consumer = new Consumer.T()
        consumer.Loop()
        0
