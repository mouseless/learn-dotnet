using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace CodeGen;

[Generator(LanguageNames.CSharp)]
public partial class JsonSchemaGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider;
        var additionalTextsProvider = context.AdditionalTextsProvider.Where(at => at.Path.EndsWith("analyzer.config.json")).Select((text, _) => text.GetText());

        var combine = compilationProvider.Combine(additionalTextsProvider.Collect());

        context.RegisterSourceOutput(combine, (spc, compilation) =>
        {
            var config = compilation.Right.FirstOrDefault();
            var analyzerConfig = config != null ? Deserialize<AnalyzerConfig>(config.ToString()) : null;

            if (analyzerConfig?.ControllerServicesNamespace == null) return;

            Execute(spc, compilation.Left, analyzerConfig);
        });
    }

    private void Execute(SourceProductionContext spc, Compilation compilation, AnalyzerConfig config)
    {
        var models = GetApplicationModel(compilation, config);

        var jsonSchema = Serialize(models);

        spc.AddSource($"Schema.generated.cs",
$@"namespace X;
static class Y
{{
    static string Z = @""
===JSON BEGIN===
{jsonSchema.Replace('"', '\'')}
===JSON END===
"";
}}
"
        );
    }

    private string Serialize(List<ServiceModel> source) => JsonConvert.SerializeObject(source);

    private T Deserialize<T>(string source) => JsonConvert.DeserializeObject<T>(source);

    private List<ServiceModel> GetApplicationModel(Compilation compilation, AnalyzerConfig config)
    {
        var result = new List<ServiceModel>();

        foreach (var tree in compilation.SyntaxTrees)
        {
            var semanticModel = compilation.GetSemanticModel(tree);

            var classDeclarations = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classDeclaration in classDeclarations)
            {
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                if (classSymbol?.ContainingNamespace?.ToString() == config.ControllerServicesNamespace &&
                    classSymbol?.DeclaredAccessibility == Accessibility.Public)
                {
                    ServiceModel applicationModel = new();

                    applicationModel.TargetNamespace = config.TargetProject;
                    applicationModel.Namespace = classSymbol.ContainingNamespace.ToString();
                    applicationModel.Name = classSymbol?.Name;

                    var methods = classSymbol.GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(m => m.MethodKind == MethodKind.Ordinary)
                        .Select(m => new Operation()
                        {
                            Name = m.Name,
                            Type = m.ReturnType.MetadataName,
                            ReturnValue = $"This is {m.Name} from {classSymbol.Name}"
                        }).ToArray();

                    applicationModel.Operations = methods;
                    result.Add(applicationModel);
                }
            }
        }

        return result;
    }
}
