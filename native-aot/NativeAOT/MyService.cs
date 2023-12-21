using System.Reflection;

namespace NativeAOT;

public class MyService(DependService _dependService, ILogger<MyService> _logger)
{
    public string MethodStringReturn()
    {
        return _dependService.MethodStringReturn();
    }

    public string MethodType()
    {
        return _dependService.MethodType();
    }

    public string MethodGenericType()
    {
        return _dependService.MethodServiceType<MyService>();
    }

    public void MethodLogging()
    {
        _logger.LogWarning("My Service Log");
    }

    public string MethodAccessAssembly()
    {
        return $@"
        EntryAssembly: {Assembly.GetEntryAssembly()}
        MyService Assembly FullName: {typeof(MyService).Assembly.FullName}
        ";
    }

    public void MethodReflection()
    {
        Type classType = Type.GetType("NativeAOT.SampleClass")!;
        ConstructorInfo constructor = classType.GetConstructor(Type.EmptyTypes)!;
        object newInstance = constructor.Invoke(null);

        Type.GetType("NativeAOT.SampleClass")!.GetMethod("GetMessage")!.Invoke(newInstance, null);
    }
}
