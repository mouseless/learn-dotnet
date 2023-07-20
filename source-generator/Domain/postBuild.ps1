param($bin, $config, $netversion, $generatepath)

$cli = "$PSScriptRoot\..\Cli\bin\$config\$netversion\Cli.exe"

& $cli "domain" "../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.generated.cs" "../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.schema.json"

if($LASTEXITCODE -gt 0)
{
    exit $LASTEXITCODE
}
