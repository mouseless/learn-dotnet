public static class ServiceProviderExtensions
{
    public static T GetRequiredServiceUsingRequestServices<T>(this IServiceProvider source) where T : notnull
    {
        var http = source.GetRequiredService<IHttpContextAccessor>();

        if (http.HttpContext is null) { return source.GetRequiredService<T>(); }

        return http.HttpContext.RequestServices.GetRequiredService<T>();
    }
}