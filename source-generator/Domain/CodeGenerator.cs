using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Domain;

[Generator]
public class CodeGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context)
    {
        string generatedCode = @"
using System;

public static class GeneratedClass
{
    public static string PrintMessage()
    {
        Console.WriteLine(""GeneratedClass method running"");
        return ""This is GeneratedClass method"";
    }
}
";

        context.AddSource("GeneratedClass.cs", SourceText.From(generatedCode, Encoding.UTF8));
    }
}
