param($bin, $config, $netversion)

$cli = "$PSScriptRoot\..\Cli\bin\$config\$netversion\Cli.exe"

& $cli '../Domain/Generated/CodeGen/CodeGen.CodeToJsonSchemaGenerator/Scheduler.generated.cs' "../Domain/bin/$config/$netversion/Scheduler.schema.json" "../WebApp/bin/$config/$netversion/Scheduler.schema.json"

if($LASTEXITCODE -gt 0)
{
    exit $LASTEXITCODE
}
