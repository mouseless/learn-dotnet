using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace LearnAssembly;

public class PersistedAssemblies
{
    public void Run()
    {
        Console.WriteLine("***Persisted Assembly***");
        Console.WriteLine("Give me an operation like 'a + b' or 'a * b': ");

        string operation = Console.ReadLine()!;

        string assemblyPath = "DynamicMathModule.dll";

        CreateAndSaveAssembly(assemblyPath, operation);

        UseSavedAssembly(assemblyPath);
    }

    static void CreateAndSaveAssembly(string assemblyPath, string operation)
    {
        PersistedAssemblyBuilder assemblyBuilder = new PersistedAssemblyBuilder(
            new AssemblyName("DynamicMathAssembly"),
            typeof(object).Assembly);

        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MathModule");
        TypeBuilder typeBuilder = moduleBuilder.DefineType("MathOperations", TypeAttributes.Public | TypeAttributes.Class);

        MethodBuilder methodBuilder = typeBuilder.DefineMethod(
            "Calculate",
            MethodAttributes.Public | MethodAttributes.Static,
            typeof(double),
            [typeof(double), typeof(double)]);

        ILGenerator il = methodBuilder.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);

        switch (Regex.Replace(operation, @"\s+", string.Empty))
        {
            case "a+b":
                il.Emit(OpCodes.Add);
                break;
            case "a-b":
                il.Emit(OpCodes.Sub);
                break;
            case "a*b":
                il.Emit(OpCodes.Mul);
                break;
            case "a/b":
                il.Emit(OpCodes.Div);
                break;
            default:
                throw new InvalidOperationException("Wrong operation");
        }

        il.Emit(OpCodes.Ret);

        typeBuilder.CreateType();
        assemblyBuilder.Save(assemblyPath);

        Console.WriteLine($"Dynamic assembly created: '{assemblyPath}'");
    }

    static void UseSavedAssembly(string assemblyPath)
    {
        Console.WriteLine($"Saved assembly '{assemblyPath}' loading...");
        Assembly assembly = Assembly.LoadFrom(assemblyPath);

        Type type = assembly.GetType("MathOperations")!;
        MethodInfo method = type.GetMethod("Calculate")!;

        Console.WriteLine("Enter first parameter:");
        double a = double.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter second parameter:");
        double b = double.Parse(Console.ReadLine()!);

        double result = (double)method.Invoke(null, [a, b])!;
        Console.WriteLine($"Result: {result}");
    }
}