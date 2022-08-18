namespace CapeMay.AdminServer

open System
open Giraffe
open Microsoft.AspNetCore.Http
open CapeMay.Domain

module Tenants =
    module private Impl =
        let private readTenants () =
            task {
                let tenant =
                    { Id = TenantId.create (Guid.NewGuid())
                      Fqdn = NonEmptyString.parse("test").Value }

                return [ tenant ]
            }

        let getTenants: HttpHandler =
            fun (next: HttpFunc) (ctx: HttpContext) ->
                task {
                    let! tenants = readTenants ()
                    return! json {| Tenants = tenants |} next ctx
                }

    [<Literal>]
    let GetTenantsPath = "/tenant"

    let routes () =
        choose [ route GetTenantsPath >=> GET >=> Impl.getTenants ]
