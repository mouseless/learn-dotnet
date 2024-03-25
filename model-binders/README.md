# Model Binders

We needed to be able to use an entity directly from an action parameter. This
research was done to learn how to utilize custom model binders to achieve this
goal.

## Result

We've decided to solve this problem through code generation instead of model
binding because of inconsistency between libraries & lack of features;

- model binding does not seem to support nested properties, which requires
  custom solution for json serializer as well
- it doesn't work well with swashbuckle, which is another thing to maintain
- microsoft does not recommend to use model binders to lookup for entities

## A Potential Solution

We've used `IModelBinder` interface to implement a custom solution. It provides
a clean base to design a custom model binding for entities.

```csharp
public class QueryModelBinder<TModel>(IQuery<TModel> _query)
    : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // ... put find entity logic here using `_query`
    }
}
```

To map a model binder for a model, we've used `IModelBinderProvider`.

```csharp
public class QueryModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        // check if given context.Metadata.ModelType is an entity
        // return query of that type using context.Services
    }
}
```

## Other Highlights

- When a value type implements `IParseable<T>`, .NET automatically makes use of
  `static abstract TryParse` method in the interface. So no need to write custom
  model binders for custom value types any more :partying_face:
- In case `IParseable` is not enough, `TypeConverter` classes is another option
  to bind a value type
- `record` types are allowed to have primary constructors, but regular `class`
  types with a primary constructor does not get automatically bound as a model
- Returning status code directly from model binders is not recommended

## Links to Follow

- [Model Binding][]
- [Custom Model Binding][]
- [Author Sample][]

[Model Binding]: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0
[Custom Model Binding]: https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-8.0
[Author Sample]: https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-8.0#custom-model-binder-sample
