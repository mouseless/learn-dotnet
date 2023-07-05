using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace Domain;

[Generator]
public class CodeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationIncrementalValue = context.CompilationProvider;

        var model = "";
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Domain.ControllerTemplate.schema.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                model = reader.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("File cannot read", ex);
        }

        ApplicationModel appModel = Desrialize<ApplicationModel>(model);

        context.RegisterSourceOutput(
            compilationIncrementalValue,
            (context, compilation) =>
            {
                // Get the entry point method
                var mainMethod = compilation.GetEntryPoint(context.CancellationToken);

                // Add the source code to the compilation
                context.AddSource($"Controller.Generated.cs", Template(appModel));
            });
    }

    private T Desrialize<T>(string source) => JsonSerializer.Deserialize<T>(source);

    private string Template(ApplicationModel model) => $@"
// Auto-generated code
using Microsoft.AspNetCore.Mvc;

namespace {model.Id};

[ApiController]
[Route("""")]
public class {model.Name} : ControllerBase
{{
    public string {model.Operations.OperationName}()
    {{
        return ""{model.Operations.ReturnValue}"";
    }}
}}
";
}
