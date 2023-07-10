using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace Domain;

[Generator]
public class CodeToSchemaJsonGenerator : IIncrementalGenerator
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
#warning .cs dosyası oluşturuluyor. .json çıktı gerekli
            spc.AddSource($"ApplicationModel.generated.schema.json", appModel);
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

                if (classSymbol?.ContainingNamespace?.ToString() == "WebApp.System")
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
