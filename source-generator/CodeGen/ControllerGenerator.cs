using Microsoft.CodeAnalysis;

namespace CodeGen;

[Generator(LanguageNames.CSharp)]
public class ControllerGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationIncrementalValue = context.CompilationProvider;
        var additionalFiles = context.AdditionalTextsProvider;

        var combine = compilationIncrementalValue.Combine(additionalFiles.Collect());

        context.RegisterSourceOutput(combine, (context, compilation) =>
            {
            var analyzerConfigText = compilation.Right
                .Where(additionalFile => additionalFile.Path.EndsWith("analyzer.config.json"))
                .Select(additionalFile => additionalFile.GetText())
                .FirstOrDefault();

            if (analyzerConfigText == null) return;

            AnalyzerConfig analyzerConfig = analyzerConfigText.Deserialize<AnalyzerConfig>();

            if (analyzerConfig.JsonSchema == null) return;

            var jsonSchemaText = compilation.Right
                .Where(additionalFile => additionalFile.Path.EndsWith(analyzerConfig.JsonSchema))
                .Select(additionalFile => additionalFile.GetText())
                .FirstOrDefault();

            var serviceModels = jsonSchemaText?.Deserialize<List<ServiceModel>>() ?? throw new ArgumentNullException();

            serviceModels.ForEach(serviceModel =>
                context.AddSource($"{serviceModel.Name}Controller.generated.cs", ControllerTemplate(serviceModel))
            );
        });
    }

    private string ControllerTemplate(ServiceModel source) =>
$@"// Auto-generated code

using Microsoft.AspNetCore.Mvc;
using {source.Namespace};

namespace {source.TargetNamespace};

[ApiController]
[Route("""")]
public class {source.Name}Controller : ControllerBase
{{
{string.Join(string.Empty, source.Operations.Select(operation =>
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