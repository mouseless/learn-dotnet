using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace CodeGen;

[Generator(LanguageNames.CSharp)]
public class ControllerGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationIncrementalValue = context.CompilationProvider;
        var additionalFiles = context.AdditionalTextsProvider;

        var combine = compilationIncrementalValue.Combine(additionalFiles.Collect());

        context.RegisterSourceOutput(
            combine,
            (context, compilation) =>
            {
            var analyzerConfigText = compilation.Right
                .Where(additionalFile => additionalFile.Path.EndsWith("analyzer.config.json"))
                .Select(additionalFile => additionalFile.GetText())
                .FirstOrDefault();

            if (analyzerConfigText == null) return;

            AnalyzerConfig analyzerConfig = Deserialize<AnalyzerConfig>(analyzerConfigText.ToString());

            if (analyzerConfig.JsonSchema == null) return;

            var jsonSchemaText = compilation.Right
                .Where(additionalFile => additionalFile.Path.EndsWith(analyzerConfig.JsonSchema))
                .Select(additionalFile => additionalFile.GetText())
                .FirstOrDefault();

            List<ServiceModel> serviceModels = Deserialize<List<ServiceModel>>(jsonSchemaText.ToString());

            serviceModels.ForEach(serviceModel =>
                context.AddSource($"{serviceModel.Name}Controller.generated.cs", serviceModel.ControllerTemplate())
                );
            });
    }

    private T Deserialize<T>(string source) => JsonConvert.DeserializeObject<T>(source);
}