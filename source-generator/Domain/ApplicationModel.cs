namespace Domain;

public class ApplicationModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Operation[] Operations { get; set; }
}

public class Operation
{
    public string Name { get; set; }
    public string Type { get; set; }
    public object ReturnValue { get; set; }
}

public static class ApplicationModelExtensions
{
    public static string ControllerTemplate(this ApplicationModel source) =>
$@"// Auto-generated code
using Microsoft.AspNetCore.Mvc;

namespace {source.Id};

[ApiController]
[Route("""")]
public class {source.Name} : ControllerBase
{{
    public string Get()
    {{
        return ""This is {source.Name}"";
    }}
    {string.Join("", source.Operations.Select(operation =>
$@"
    [HttpGet(""/{operation.Name}"")]
    public {operation.Type} {operation.Name}()
    {{
        return ""{operation.ReturnValue}"";
    }}"
        )
     )}
}}";
}