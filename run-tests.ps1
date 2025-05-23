# Runs every input/output test in tests/ against a Release build of the game.
# Each tests/NN-name.in is fed to the program and its output is compared,
# byte by byte, with tests/NN-name.out.
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot

dotnet build src/BatalhaNaval/BatalhaNaval.csproj -c Release -nologo -v q | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Host 'Build failed'; exit 1 }
$app = Join-Path $PSScriptRoot 'src/BatalhaNaval/bin/Release/net8.0/BatalhaNaval.dll'

$passed = 0; $failed = 0
foreach ($input in Get-ChildItem tests -Filter *.in | Sort-Object Name) {
    $expectedPath = [IO.Path]::ChangeExtension($input.FullName, '.out')
    $actualPath = [IO.Path]::GetTempFileName()

    # Feed the input file through cmd so the bytes reach the program unchanged
    cmd /c "dotnet `"$app`" < `"$($input.FullName)`" > `"$actualPath`""

    $expected = [IO.File]::ReadAllBytes($expectedPath)
    $actual = [IO.File]::ReadAllBytes($actualPath)
    $same = ($expected.Length -eq $actual.Length) -and
            (-not (Compare-Object $expected $actual -SyncWindow 0))

    if ($same) {
        Write-Host "PASS  $($input.BaseName)"
        $passed++
    } else {
        Write-Host "FAIL  $($input.BaseName)"
        $failed++
    }
    Remove-Item $actualPath
}

Write-Host "$passed passed, $failed failed"
if ($failed -gt 0) { exit 1 }
