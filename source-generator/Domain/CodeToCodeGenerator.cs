using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Domain;

[Generator]
public class CodeToCodeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationIncrementalValue = context.CompilationProvider;

        var syntaxTree = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (c, _) => c is ClassDeclarationSyntax,
                transform: (n, _) => (ClassDeclarationSyntax)n.Node
            ).Where(m => m is not null);

        IncrementalValueProvider<(Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right)> compilation = compilationIncrementalValue.Combine(syntaxTree.Collect());

        context.RegisterSourceOutput(compilation, (spc, source) => 
            Execute(spc, source.Left, source.Right));
    }

    private void Execute(SourceProductionContext spc, Compilation left, ImmutableArray<ClassDeclarationSyntax> right)
    {
        var controllerCandidates = right.Where(@class => @class.SyntaxTree.ToString().Contains("namespace WebApp.System;")).FirstOrDefault();
    }
}
