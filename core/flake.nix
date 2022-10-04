{
  description = "Cape May";

  inputs.nixpkgs.url = "nixpkgs/nixpkgs-unstable";

  outputs = inputs@{ self, nixpkgs }:
    let
      supportedSystems =
        [ "x86_64-linux" "x86_64-darwin" "aarch64-linux" "aarch64-darwin" ];

      forAllSystems = nixpkgs.lib.genAttrs supportedSystems;

      nixpkgsFor = forAllSystems (system:
        import nixpkgs {
          inherit system;
          overlays = [
            (import ../nix/overlays/sqitch.nix)
            (import ../nix/overlays/buildPaketDotnetModule.nix)
          ];
        });
    in {
      packages = forAllSystems (system:
        let pkgs = nixpkgsFor.${system};
        in {
          Vp.FSharp.Sql.Sqlite = pkgs.buildCmDotnetModule {
            name = "Vp.FSharp.Sql.Sqlite";
            projectFile =
              "src/Vp.FSharp.Sql.Sqlite/Vp.FSharp.Sql.Sqlite/Vp.FSharp.Sql.Sqlite.fsproj";
          };
          CapeMay.Domain = pkgs.buildPaketDotnetModule {
            name = "CapeMay.Domain";
            version = "0.0.1";
          };
        });

      devShells = forAllSystems (system:
        let pkgs = nixpkgsFor.${system};
        in {
          default = with pkgs;
            mkShell {
              packages = [
                dotnetCorePackages.sdk_6_0
                mono
                sqlite
                sqitch
                just
                flyctl
                nuget-to-nix
                nixpacks
              ];
            };
        });
    };
}
