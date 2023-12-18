public static class ServiceProviderExtensions
{
    public static T GetRequiredServiceUsingRequestServices<T>(this IServiceProvider source) where T : notnull
    => (T)source.GetRequiredServiceUsingRequestServices(typeof(T));

    public static object GetRequiredServiceUsingRequestServices(this IServiceProvider source, Type type)
    {
        var http = source.GetRequiredService<IHttpContextAccessor>();

        if (http.HttpContext is null) { return source.GetRequiredService(type); }

        var context = http.HttpContext;
        var serviceProvider = context.RequestServices;

        return serviceProvider.GetRequiredService(type);
    }

    public static void AddTransientWithFactory<TService>(this IServiceCollection source) where TService : class =>
        source.AddTransientWithFactory<TService, TService>();

    public static void AddTransientWithFactory<TService, TImplementation>(this IServiceCollection source)
        where TService : class
        where TImplementation : class, TService
    {
        source.AddSingleton<Func<TService>>(sp => () => sp.GetRequiredServiceUsingRequestServices<TService>());
        source.AddTransient<TService, TImplementation>();
    }

    public static void AddScopedWithFactory<TService>(this IServiceCollection source) where TService : class =>
        source.AddScopedWithFactory<TService, TService>();

    public static void AddScopedWithFactory<TService, TImplementation>(this IServiceCollection source)
        where TService : class
        where TImplementation : class, TService
    {
        source.AddSingleton<Func<TService>>(sp => () => sp.GetRequiredServiceUsingRequestServices<TService>());
        source.AddScoped<TService, TImplementation>();
    }
}
