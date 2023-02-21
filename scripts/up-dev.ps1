$CURRENTDIR = Split-Path -parent $MyInvocation.MyCommand.Definition
$TARGETDIR = Resolve-Path -Path "$CURRENTDIR/../src"
Set-Location $TARGETDIR
docker compose build
docker compose up -d