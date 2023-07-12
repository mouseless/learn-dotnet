using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace CodeGen;

[Generator(LanguageNames.CSharp)]
public class CodeToJsonSchemaGenerator : IIncrementalGenerator
{
    class AnalyzerConfig
    {
        public string ControllerServicesNamespace { get; set; }
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider;
        var additionalTextsProvider = context.AdditionalTextsProvider.Where(at => at.Path.EndsWith("analyzer.config.json")).Select((text, _) => text.GetText());

        var combine = compilationProvider.Combine(additionalTextsProvider.Collect());

        context.RegisterSourceOutput(combine, (spc, compilation) =>
        {
            var config = compilation.Right.FirstOrDefault();
            var analyzerConfig = config != null ? Deserialize<AnalyzerConfig>(config.ToString()) : null;
            var @namespace = analyzerConfig != null ? analyzerConfig.ControllerServicesNamespace : "";

            Execute(spc, compilation.Left, @namespace);
        });
    }

    private void Execute(SourceProductionContext spc, Compilation compilation, string texts)
    {
        var models = GetApplicationModel(compilation, texts);

        foreach (var model in models)
        {
            string appModel = Serialize(model);

            spc.AddSource($"{model.Name}.generated.cs", $@"
namespace X;

static class Y
{{
    static string Z = @""
===JSON BEGIN===
{appModel.Replace('"', '\'')}
===JSON END===
"";
}}
");
        }
    }

    private string Serialize(ApplicationModel source) => JsonConvert.SerializeObject(source);

    private T Deserialize<T>(string source) => JsonConvert.DeserializeObject<T>(source);

    private List<ApplicationModel> GetApplicationModel(Compilation compilation, string @namespace)
    {
        var result = new List<ApplicationModel>();

        foreach (var tree in compilation.SyntaxTrees)
        {
            var semanticModel = compilation.GetSemanticModel(tree);

            var classDeclarations = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classDeclaration in classDeclarations)
            {
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                if (classSymbol?.ContainingNamespace?.ToString() == @namespace &&
                    classSymbol?.DeclaredAccessibility == Accessibility.Public)
                {
                    ApplicationModel applicationModel = new();

                    applicationModel.Id = classSymbol?.ContainingNamespace?.ToString();
                    applicationModel.Name = classSymbol?.Name;

                    var methods = classSymbol.GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(m => m.MethodKind == MethodKind.Ordinary)
                        .Select(m => new Operation()
                        {
                            Name = m.Name,
                            Type = m.ReturnType.Name,
                            ReturnValue = $"Return {m.ReturnType.Name}"
                        }).ToArray();

                    applicationModel.Operations = methods;
                    result.Add(applicationModel);
                }
            }
        }

        return result;
    }
}
