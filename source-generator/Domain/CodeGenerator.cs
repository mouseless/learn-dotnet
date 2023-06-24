using Microsoft.CodeAnalysis;

namespace Domain;

[Generator]
public class CodeGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        // Code generation goes here
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}