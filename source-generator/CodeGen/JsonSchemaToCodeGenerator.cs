using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace CodeGen;

[Generator(LanguageNames.CSharp)]
public class JsonSchemaToCodeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationIncrementalValue = context.CompilationProvider;
        var additionalFiles = context.AdditionalTextsProvider.Where(w => w.Path.EndsWith("ControllerTemplate.schema.json")).Select((s, _) => s.GetText());

        var combine = compilationIncrementalValue.Combine(additionalFiles.Collect());

        context.RegisterSourceOutput(
            combine,
            (context, compilation) =>
            {
                var mainMethod = compilation.Left.GetEntryPoint(context.CancellationToken);
                var text = compilation.Right.First().ToString();
                ApplicationModel appModel = Deserialize<ApplicationModel>(text);

                context.AddSource($"Controller.generated.cs", appModel.ControllerTemplate());
            });
    }

    private T Deserialize<T>(string source) => JsonConvert.DeserializeObject<T>(source);
}
