namespace CodeGen;

public static class ModelExtensions
{
    public static string ControllerTemplate(this ServiceModel source) =>
$@"// Auto-generated code

using Microsoft.AspNetCore.Mvc;
using {source.Namespace};

namespace {source.TargetNamespace};

[ApiController]
[Route("""")]
public class {source.Name}Controller : ControllerBase
{{
    {string.Join("", source.Operations.Select(operation =>
$@"
    [HttpGet(""/{source.Name}/{operation.Name}"")]
    public {operation.Type} {operation.Name}()
    {{
        return new {source.Name}().{operation.Name}();
    }}"
        )
     )}
}}";

    public static string ServiceModelTemplateAsCs(this string source) =>
$@"namespace X;
static class Y
{{
    static string Z = @""
===JSON BEGIN===
{source.Replace('"', '\'')}
===JSON END===
"";
}}
";
}