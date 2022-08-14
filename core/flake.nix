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
    in {
      packages = forAllSystems
        (system: let pkgs = nixpkgsFor.${system}; in { default = { }; });

      devShells = forAllSystems (system:
        let pkgs = nixpkgsFor.${system};
        in {
          default = pkgs.mkShell {
            buildInputs = with pkgs; [ dotnet-sdk mono sqlite sqitch ];
          };
        });
    };
}
