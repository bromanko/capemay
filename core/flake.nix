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
          overlays = [ (import ../nix/overlays/sqitch.nix) ];
        });

      buildCMDotnetModule = (attrs:
        let
          projectFile = attrs.projectFile or ("src/" + attrs.pname + "/"
            + attrs.pname + ".fsproj");
        in attrs.pkgs.buildDotnetModule {
          pname = attrs.pname;
          version = attrs.version or "0.0.1";
          src = ./.;
          projectFile = projectFile;
          nugetDeps = ./nix + "/${attrs.pname}" + ".deps.nix";
          dotnet-sdk = attrs.pkgs.dotnetCorePackages.sdk_6_0;
        });
    in {
      packages = forAllSystems (system:
        let pkgs = nixpkgsFor.${system};
        in {
          Vp.FSharp.Sql.Sqlite = buildCMDotnetModule {
            inherit pkgs;
            pname = "Vp.FSharp.Sql.Sqlite";
            projectFile =
              "src/Vp.FSharp.Sql.Sqlite/Vp.FSharp.Sql.Sqlite/Vp.FSharp.Sql.Sqlite.fsproj";
          };
          CapeMay.Domain = buildCMDotnetModule {
            inherit pkgs;
            pname = "CapeMay.Domain";
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
