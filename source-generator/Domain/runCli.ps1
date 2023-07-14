param($bin, $config, $netversion)

$cli = "$PSScriptRoot\..\Cli\bin\$config\$netversion\Cli.exe"

& $cli "../Domain/bin/Analyzer/$netversion/generated" "../WebApp/GeneratedController"

if($LASTEXITCODE -gt 0)
{
    exit $LASTEXITCODE
}
