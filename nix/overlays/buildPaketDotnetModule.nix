self: super:

let pkgs = self.pkgs;
in {
  buildCmDotnetModule = (attrs@{ name
    , projectFile ? ("src/" + attrs.name + "/" + attrs.name + ".fsproj")
    , version ? "0.0.1", src ? ./.
    , nugetDeps ? ./nix + "/${attrs.name}" + ".deps.nix", ... }:
    pkgs.callPackage pkgs.buildDotnetModule attrs);

  buildPaketDotnetModule = with pkgs;
    (attrs@{ name ? "${args.pname}-${args.version}", pname ? name }:
      (callPackage buildDotnetModule attrs).overrideAttrs (finalAttrs: {
        passthru = finalAttrs.passthru // {
          # This is originally sourced from https://github.com/NixOS/nixpkgs/blob/master/pkgs/build-support/dotnet/build-dotnet-module/default.nix
          fetch-deps = let
            # Derivations may set flags such as `--runtime <rid>` based on the host platform to avoid restoring/building nuget dependencies they dont have or dont need.
            # This introduces an issue; In this script we loop over all platforms from `meta` and add the RID flag for it, as to fetch all required dependencies.
            # The script would inherit the RID flag from the derivation based on the platform building the script, and set the flag for any iteration we do over the RIDs.
            # That causes conflicts. To circumvent it we remove all occurances of the flag.
            flags = let
              hasRid = flag:
                lib.any (v: v) (map (rid: lib.hasInfix rid flag)
                  (lib.attrValues finalAttrs.dotnet-sdk.runtimeIdentifierMap));
            in builtins.filter (flag: !(hasRid flag))
            (finalAttrs.dotnetFlags ++ finalAttrs.dotnetRestoreFlags);
          in writeShellScript "fetch-${pname}-deps" ''
            set -euo pipefail
            export PATH="${
              lib.makeBinPath [
                coreutils
                finalAttrs.dotnet-sdk
                (nuget-to-nix.override { inherit (finalAttrs.dotnet-sdk) ; })
              ]
            }"
            for arg in "$@"; do
                case "$arg" in
                    --keep-sources|-k)
                        keepSources=1
                        shift
                        ;;
                    --help|-h)
                        echo "usage: $0 [--keep-sources] [--help] <output path>"
                        echo "    <output path>   The path to write the lockfile to. A temporary file is used if this is not set"
                        echo "    --keep-sources  Dont remove temporary directories upon exit, useful for debugging"
                        echo "    --help          Show this help message"
                        exit
                        ;;
                esac
            done
            export tmp=$(mktemp -td "${pname}-tmp-XXXXXX")
          '';
        };
      }));
}
