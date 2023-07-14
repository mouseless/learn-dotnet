namespace CodeGen;

public class ServiceModel
{
    public string Namespace { get; set; }
    public string TargetNamespace { get; set; }
    public string Name { get; set; }
    public Operation[] Operations { get; set; }
}

public static class ServiceModelExtensions
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
}