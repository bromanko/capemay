namespace CapeMay.Admin.Server

open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Diagnostics.HealthChecks
open Giraffe

module Health =
    let responseWriter (ctx: HttpContext) (report: HealthReport): Task =
        ctx.WriteJsonAsync report
