self: super:

let
  pkgs = self.pkgs;
  perlSqitch = pkgs.perlPackages.AppSqitch;
  modules = with pkgs.perlPackages; [ DBDSQLite DBDPg ];
in rec {
  buildPaketDotnetModule =
    pkgs.buildDotnetModule.override { passthru.fetch-deps = "test"; };
}
