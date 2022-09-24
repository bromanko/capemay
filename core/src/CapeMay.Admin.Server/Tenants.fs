namespace CapeMay.Admin.Server

open Giraffe
open CapeMay.Admin.Domain
open CapeMay.Admin.Domain.Parsing
open CapeMay.Admin.Server

module Tenants =
    [<AllowNullLiteral>]
    type CreateTenantDto() =
        member val Fqdn = "" with get, set

    module Create =
        open FsToolkit.ErrorHandling.Operator.Validation

        let mkCreateTenant fqdn =
            { CreateTenant.Fqdn = fqdn
              Id = TenantId.create () }

        let parse (req: CreateTenantDto) =
            mkCreateTenant
            <!> tryParseFqdn req.Fqdn "FQDN is invalid."

        let exec (compRoot: CompositionRoot.T) t =
            Exec.create (fun _ -> compRoot.Commands.Tenants.Create t)

    [<Literal>]
    let TenantPath = "/tenant"

    let routes compRoot =
        choose [ route TenantPath
                 >=> POST
                 >=> bindAndParse Create.parse (Create.exec compRoot)
                 route TenantPath
                 >=> GET
                 >=> warbler (fun _ ->
                     Exec.read compRoot.Commands.Tenants.GetAll) ]
