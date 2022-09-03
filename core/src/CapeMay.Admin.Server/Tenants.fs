namespace CapeMay.Admin.Server

open System
open Giraffe
open Microsoft.AspNetCore.Http
open CapeMay.Admin.Domain
open CapeMay.Domain

module Tenants =
    [<AllowNullLiteral>]
    type CreateTenantDto() =
        member val Fqdn = "" with get, set

    type CreateTenant = { Fqdn: NonEmptyString.T; Id: TenantId.T }

    module private Impl =
        module Read =
            let private readTenants () =
                task {
                    // TODO read this from the db
                    let tenant =
                        { Id = TenantId.create()
                          Fqdn = NonEmptyString.parse("test").Value }

                    return [ tenant ]
                }

            let getTenants: HttpHandler =
                fun (next: HttpFunc) (ctx: HttpContext) ->
                    task {
                        let! tenants = readTenants ()
                        return! json {| Tenants = tenants |} next ctx
                    }

        module Create =
            open Chessie.ErrorHandling

            let mkCreateTenant fqdn = { Fqdn = fqdn; Id = TenantId.create() }

            let parse (req: CreateTenantDto) =
                mkCreateTenant
                <!> tryParseNES req.Fqdn "FQDN is required."

            let createTenant (req: CreateTenant) : HttpHandler =
                fun (next: HttpFunc) (ctx: HttpContext) ->
                    task {
                        // TODO persist to the database
                        // TODO return the proper object
                        return! Successful.created (json req) next ctx
                    }

    [<Literal>]
    let TenantPath = "/tenant"

    let routes () =
        choose [ route TenantPath
                 >=> POST
                 >=> bindAndParse Impl.Create.parse Impl.Create.createTenant
                 route TenantPath >=> GET >=> Impl.Read.getTenants ]
