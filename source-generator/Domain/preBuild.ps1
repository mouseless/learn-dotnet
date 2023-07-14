param($bin, $config, $netversion)

$cli = "$PSScriptRoot\..\Cli\bin\$config\$netversion\Cli.exe"

& $cli "pre" "../Domain/bin/Analyzer/$netversion/generated/CodeGen/CodeGen.JsonSchemaGenerator/Schema.generated.cs" "../Domain/Schema.schema.json"

if($LASTEXITCODE -gt 0)
{
    exit $LASTEXITCODE
}
