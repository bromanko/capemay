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

      # buildDotnetModule = project: { };
    in {
      packages = forAllSystems (system:
        let pkgs = nixpkgsFor.${system};
        in with pkgs; {
          CapeMay.Domain = buildDotnetModule rec {
            pname = "CapeMay.Domain";
            version = "0.0.1";
            src = ./.;
            projectFile = "src/" + pname + "/" + pname + ".fsproj";
            nugetDeps = ./nix + "/${pname}" + ".deps.nix";
            dotnet-sdk = dotnetCorePackages.sdk_6_0;
            buildInputs = [ dotnetPackages.Paket ];
          };
          adminServer = buildDotnetModule {
            pname = "CapeMay Admin Server";
            version = "0.0.1";
            src = ./.;
            projectFile =
              "src/CapeMay.Admin.Server/CapeMay.Admin.Server.fsproj";
            nugetDeps = ./nix/CapeMay.Admin.Server.deps.nix;
            dotnet-sdk = dotnetCorePackages.sdk_6_0;
            executables = [ "CapeMay.Admin.Server" ];
            runtimeDeps = [ sqlite ];
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
