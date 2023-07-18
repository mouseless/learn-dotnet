param($bin, $config, $netversion, $generatepath)

$cli = "$PSScriptRoot\..\Cli\bin\$config\$netversion\Cli.exe"

& $cli "webapp" "../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.schema.json" "../WebApp/obj/$config/$netversion/Controller.schema.json"

if($LASTEXITCODE -gt 0)
{
    exit $LASTEXITCODE
}
