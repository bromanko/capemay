namespace CapeMay.Admin.Domain

open System
open System.Diagnostics
open System.Text
open System.Text.RegularExpressions
open CapeMay.Domain
open TryParse
open FsToolkit.ErrorHandling.Operator.Result

[<RequireQualifiedAccess>]
module Sqitch =
    type private ProcessResult =
        { ExitCode: int
          StdErr: string
          StdOut: string }

    let private execSqitch
        (wrkDir: NonEmptyString.T)
        (target: NonEmptyString.T)
        (cmd: NonEmptyString.T)
        =
        try
            let psi = ProcessStartInfo()

            psi.WorkingDirectory <- NonEmptyString.value wrkDir
            psi.FileName <- "sqitch"

            psi.Arguments <-
                $"{NonEmptyString.value cmd} {NonEmptyString.value target}"

            psi.UseShellExecute <- false
            psi.RedirectStandardOutput <- true
            psi.RedirectStandardError <- true
            psi.CreateNoWindow <- true

            let p = Process.Start psi
            let output = StringBuilder()
            let error = StringBuilder()
            p.OutputDataReceived.Add(fun a -> output.AppendLine a.Data |> ignore)
            p.ErrorDataReceived.Add(fun a -> error.AppendLine a.Data |> ignore)
            p.BeginOutputReadLine()
            p.BeginErrorReadLine()
            p.WaitForExit()

            Ok
                { ExitCode = p.ExitCode
                  StdErr = stderr.ToString()
                  StdOut = output.ToString() }
        with ex ->
            Error <| DomainError.UnhandledException ex


    type SqitchStatus =
        { Project: string option
          Change: string option
          Name: string option
          Deployed: DateTimeOffset option
          By: string option }

    let (|FrontMatter|_|) (input: string) =
        let m =
            Regex.Match(input, "^# (?<key>[\w\d\s]+)\:\s+(?<val>.+)$")

        if m.Success then
            (m.Groups[ "key" ].Value.Trim(), m.Groups[ "val" ].Value.Trim())
            |> Some
        else
            None

    let rec private parseStatusLines lines (acc: Map<string, string>) =
        match lines with
        | hd :: tl ->
            let acc =
                match hd with
                | FrontMatter pair -> acc.Add(fst pair, snd pair)
                | _ -> acc

            parseStatusLines tl acc
        | [] -> acc

    let tryFindDateTimeOffset (map: Map<string, string>) key =
        map.TryFind key |> Option.bind parseDateTimeOffset //(fun v -> parseDateTimeOffset)

    let mkStatus (map: Map<string, string>) =
        { Project = map.TryFind "Project"
          Change = map.TryFind "Change"
          Name = map.TryFind "Name"
          Deployed = tryFindDateTimeOffset map "Deployed"
          By = map.TryFind "By" }

    let parseStatus (stdout: string) =
        (stdout.Split "\n"
         |> Array.toList
         |> parseStatusLines) (Map<string, string> [])
        |> mkStatus

    let status wkDir target : Result<SqitchStatus option, DomainError> =
        execSqitch wkDir target (NonEmptyString.parse "status").Value
        |> Result.map (fun pr -> parseStatus pr.StdOut)
        |> Result.map (function
            | { Project = None
                Change = None
                Name = None
                Deployed = None
                By = None } -> None
            | ss -> Some ss)
