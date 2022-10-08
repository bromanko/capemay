self: super:

let
  pkgs = self.pkgs;
  perlSqitch = pkgs.perlPackages.AppSqitch;
  modules = with pkgs.perlPackages; [ DBDSQLite DBDPg ];
in rec {
  sqitch = pkgs.stdenv.mkDerivation {
    pname = "sqitch";
    version = perlSqitch.version;

    nativeBuildInputs = [ pkgs.makeWrapper ]
      ++ pkgs.lib.optional pkgs.stdenv.isDarwin pkgs.shortenPerlShebang;

    src = perlSqitch;
    dontBuild = true;

    installPhase = ''
      mkdir -p $out/bin
      for d in bin/sqitch etc lib share ; do
        # make sure dest alreay exists before symlink
        # this prevents installing a broken link into the path
        if [ -e ${perlSqitch}/$d ]; then
          ln -s ${perlSqitch}/$d $out/$d
        fi
      done
    '' + pkgs.lib.optionalString pkgs.stdenv.isDarwin ''
      shortenPerlShebang $out/bin/sqitch
    '';
    dontStrip = true;
    postFixup = ''
      wrapProgram $out/bin/sqitch --prefix PERL5LIB : ${
        pkgs.perlPackages.makeFullPerlPath modules
      }
    '';

    meta = {
      inherit (perlSqitch.meta) description homepage license platforms;
    };
  };
}
