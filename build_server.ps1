param(
    [string]$FlaxEditor = "C:\Program Files (x86)\Flax\Flax_1.12\Binaries\Editor\Win64\Release\FlaxEditor.exe",

    [string]$Project = "$PSScriptRoot",

# Examples:
# "Development.Windows"
# "Development.Win64Server"
# "Release.Windows"
    [string]$BuildTarget = "Development.Win64Server",

    [switch]$ClearCooker,
    [switch]$ClearCache
)

$ErrorActionPreference = "Stop"

function Fail($message) {
    Write-Host ""
    Write-Host "ERROR: $message" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $FlaxEditor)) {
    Fail "FlaxEditor.exe not found: $FlaxEditor"
}

if (-not (Test-Path $Project)) {
    Fail "Project path not found: $Project"
}

# Try to find a .flaxproj in the project directory.
$projectFile = Get-ChildItem -Path $Project -Filter "*.flaxproj" -File | Select-Object -First 1

if (-not $projectFile) {
    Fail "No .flaxproj file found in: $Project"
}

$argsList = @(
    "-project", "`"$($projectFile.FullName)`"",
    "-headless",
    "-mute",
    "-null",
    "-std",
    "-build", "`"$BuildTarget`""
)

if ($ClearCooker) {
    $argsList += "-clearCooker"
}

if ($ClearCache) {
    $argsList += "-clearCache"
}

Write-Host "Building Flax project..."
Write-Host "Editor:  $FlaxEditor"
Write-Host "Project: $($projectFile.FullName)"
Write-Host "Target:  $BuildTarget"
Write-Host ""

$process = Start-Process `
    -FilePath $FlaxEditor `
    -ArgumentList $argsList `
    -NoNewWindow `
    -Wait `
    -PassThru

Write-Host ""
Write-Host "Flax build exited with code: $($process.ExitCode)"

if ($process.ExitCode -ne 0) {
    exit $process.ExitCode
}

Write-Host "Build finished successfully."
