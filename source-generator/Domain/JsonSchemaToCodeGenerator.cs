using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Reflection;

namespace Domain;

[Generator(LanguageNames.CSharp)]
public class JsonSchemaToCodeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationIncrementalValue = context.CompilationProvider;
        
        var schema = ReadResource("Domain.ControllerTemplate.schema.json");

        context.RegisterSourceOutput(
            compilationIncrementalValue,
            (context, compilation) =>
            {
                var mainMethod = compilation.GetEntryPoint(context.CancellationToken);
                ApplicationModel appModel = Deserialize<ApplicationModel>(schema);

                context.AddSource($"Controller.generated.cs", appModel.ControllerTemplate());
            });
    }

    private T Deserialize<T>(string source) => JsonConvert.DeserializeObject<T>(source);

    private string ReadResource(string resourceName)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("File cannot read", ex);
        }
    }
}
