# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/FFT_Egg_Control/*" -Force -Recurse
dotnet publish "./FFT_Egg_Control.csproj" -c Release -o "$env:RELOADEDIIMODS/FFT_Egg_Control" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location