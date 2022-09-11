namespace CapeMay.Domain

type DomainError =
    | UniquenessError of string
    | UnhandledException of exn
