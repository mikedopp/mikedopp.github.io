$ErrorActionPreference = 'Stop'

Push-Location $PSScriptRoot
try {
  dotnet run --project .\tools\MikeSiteEditor\MikeSiteEditor.csproj
}
finally {
  Pop-Location
}
