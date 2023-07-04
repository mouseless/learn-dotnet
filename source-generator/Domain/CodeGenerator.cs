using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Reflection;
using System.Text;

namespace Domain;

[Generator]
public sealed class CodeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationIncrementalValue = context.CompilationProvider;

        context.RegisterSourceOutput(
            compilationIncrementalValue,
            static (context, compilation) =>
            {
                // Get the entry point method
                var mainMethod = compilation.GetEntryPoint(context.CancellationToken);

                string source = $@"
// Auto-generated code
using Microsoft.AspNetCore.Mvc;

namespace WebApp;
    
[ApiController]
[Route("""")]
public class TestController : ControllerBase
{{
    public string Get()
    {{
        return ""Returning from TestController Get Method"";
    }}
}}
                ";

                // Add the source code to the compilation
                context.AddSource($"Controller.Generated.cs", source);
            });
    }
}