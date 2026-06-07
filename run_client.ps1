$ErrorActionPreference = "Stop"

$exe = Join-Path $PSScriptRoot "Output\Win64Client\Bangerang.exe"

Write-Host "Starting Bangerang client..."
Write-Host "Exe: $exe"

Start-Process `
    -FilePath $exe `
    -ArgumentList "-std" `
    -NoNewWindow `
    -Wait
