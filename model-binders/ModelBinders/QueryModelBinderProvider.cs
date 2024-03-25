using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ModelBinders;

public class QueryModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var type = context.Metadata.ModelType;
        if (type.IsClass && !type.IsAbstract &&
            type.GetConstructors().Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(Guid) && p.Name == "_id"))
        )
        {
            return (IModelBinder?)context.Services.GetRequiredService(typeof(QueryModelBinder<>).MakeGenericType(type));
        }

        return null;
    }
}
