namespace CapeMay.Admin.Domain

open CapeMay.Domain

module Parsing =
    let tryParseFqdn<'T, 'TErr> v (msg: 'TErr) = tryParse Fqdn.parse v msg
