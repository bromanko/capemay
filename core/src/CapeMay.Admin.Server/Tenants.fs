namespace CapeMay.Admin.Server

open Giraffe
open Microsoft.AspNetCore.Http
open CapeMay.Admin.Domain
open CapeMay.Domain
open CapeMay.Admin.Server

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

            let mkCreateTenant fqdn =
                { CreateTenant.Fqdn = fqdn
                  Id = TenantId.create () }

            let parse (req: CreateTenantDto) =
                mkCreateTenant
                <!> tryParseNES req.Fqdn "FQDN is required."

            let createTenant
                (compRoot: CompositionRoot.T)
                (t: CreateTenant)
                : HttpHandler =
                fun (next: HttpFunc) (ctx: HttpContext) ->
                    task {
                        let conn = compRoot.DataSource.MakeConnection()
                        do! conn.OpenAsync()
                        do! compRoot.DataSource.CreateTenant t conn

                        return! Successful.created (json t) next ctx
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
