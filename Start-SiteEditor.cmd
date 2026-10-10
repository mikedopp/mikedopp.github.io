@echo off
pushd "%~dp0"
dotnet run --project ".\tools\MikeSiteEditor\MikeSiteEditor.csproj"
popd
