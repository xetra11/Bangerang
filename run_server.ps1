$ErrorActionPreference = "Stop"

$exe = Join-Path $PSScriptRoot "Output\Win64Server\Bangerang.exe"

Write-Host "Starting Bangerang server..."
Write-Host "Exe: $exe"

Start-Process `
    -FilePath $exe `
    -ArgumentList "-std" `
    -NoNewWindow `
    -Wait
