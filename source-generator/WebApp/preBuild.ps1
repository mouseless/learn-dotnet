param($bin, $config, $netversion)

$cli = "$PSScriptRoot\..\Cli\bin\$config\$netversion\Cli.exe"

& $cli "webapp" "../Domain/Schema.schema.json" "../WebApp/Schema.schema.json"

if($LASTEXITCODE -gt 0)
{
    exit $LASTEXITCODE
}
