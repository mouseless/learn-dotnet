public static class ServiceProviderExtensions
{
    public static T GetRequiredServiceUsingRequestServices<T>(this IServiceProvider source) where T : notnull
    {
        var http = source.GetRequiredService<IHttpContextAccessor>();

        if (http.HttpContext is null) { return source.GetRequiredService<T>(); }

        return http.HttpContext.RequestServices.GetRequiredService<T>();
    }

    public static void AddTransientWithFactory<T>(this IServiceCollection source) where T : class
    {
        source.AddSingleton<Func<T>>(sp => () => sp.GetRequiredServiceUsingRequestServices<T>());
        source.AddTransient<T>();
    }

    public static void AddTransientWithFactory(this IServiceCollection source, Type type)
    {
        var funcType = typeof(Func<>).MakeGenericType(type);

        source.AddSingleton(funcType, sp => () => sp.GetRequiredService(type));
        source.AddTransient(type);
    }
}
