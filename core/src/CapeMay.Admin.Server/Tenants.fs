namespace CapeMay.Admin.Server

open Giraffe
open Microsoft.AspNetCore.Http
open CapeMay.Admin.Domain
open CapeMay.Admin.Domain.Parsing
open CapeMay.Admin.Server
open CapeMay.Admin.Server.Errors

module Tenants =
    [<AllowNullLiteral>]
    type CreateTenantDto() =
        member val Fqdn = "" with get, set

    module private Impl =
        module Read =
            let private readTenants () =
                task {
                    // TODO read this from the db
                    let tenant =
                        { Id = TenantId.create ()
                          Fqdn = Fqdn.parse("test.foo.com").Value }

                    return [ tenant ]
                }

            let getTenants: HttpHandler =
                fun (next: HttpFunc) (ctx: HttpContext) ->
                    task {
                        let! tenants = readTenants ()
                        return! json {| Tenants = tenants |} next ctx
                    }

        module Create =
            open FsToolkit.ErrorHandling.Operator.Validation

            let mkCreateTenant fqdn =
                { CreateTenant.Fqdn = fqdn
                  Id = TenantId.create () }

            let parse (req: CreateTenantDto) =
                mkCreateTenant
                <!> tryParseFqdn req.Fqdn "FQDN is invalid."

            let createTenant
                (compRoot: CompositionRoot.T)
                (t: CreateTenant)
                : HttpHandler =
                fun (next: HttpFunc) (ctx: HttpContext) ->
                    task {
                        match! compRoot.Commands.Tenants.CreateAsync t with
                        | Ok t -> return! Successful.created (json t) next ctx
                        | Error err -> return! respForDomainErr err next ctx
                    }

    [<Literal>]
    let TenantPath = "/tenant"

    let routes compRoot =
        choose [ route TenantPath
                 >=> POST
                 >=> bindAndParse
                         Impl.Create.parse
                         (Impl.Create.createTenant compRoot)
                 route TenantPath >=> GET >=> Impl.Read.getTenants ]
