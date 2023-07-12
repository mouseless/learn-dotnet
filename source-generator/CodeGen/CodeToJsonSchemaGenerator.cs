using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace CodeGen;

[Generator(LanguageNames.CSharp)]
public class CodeToJsonSchemaGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider;

        context.RegisterSourceOutput(compilationProvider, (spc, compilation) =>
            Execute(spc, compilation));
    }

    private void Execute(SourceProductionContext spc, Compilation compilation)
    {
        var models = GetApplicationModel(compilation);

        foreach (var model in models)
        {
            string appModel = Serialize(model);

            spc.AddSource("generated.cs", $@"
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

    private List<ApplicationModel> GetApplicationModel(Compilation context)
    {
        var result = new List<ApplicationModel>();

        foreach (var tree in context.SyntaxTrees)
        {
            var semanticModel = context.GetSemanticModel(tree);

            var classDeclarations = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

            foreach (var classDeclaration in classDeclarations)
            {
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                // ToDo - WebApp.System bilgisi csprojdan alınacak
                if (classSymbol?.ContainingNamespace?.ToString() == "WebApp.System" &&
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
