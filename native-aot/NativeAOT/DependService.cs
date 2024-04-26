namespace NativeAOT;

public class DependService
{
    public string MethodStringReturn()
    {
        return "Method 1";
    }

    public string MethodType()
    {
        return GetType().Name;
    }

    public string MethodServiceType<T>()
    {
        return typeof(T).FullName!;
    }
}