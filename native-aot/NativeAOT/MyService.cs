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

    public object MethodDynamic(dynamic param)
    {
        param.MyService = "My Service";
        return param;
    }

    // TODO - reflection örneği
    public string AccessAssembly()
    {
        return $@"
        EntryAssembly: {Assembly.GetEntryAssembly()}
        MyService Assembly FullName: {typeof(MyService).Assembly.FullName}
        ";
    }
}
