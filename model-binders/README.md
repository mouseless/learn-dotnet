# Model Binders

We needed to be able to use an entity directly from an action parameter. This
research was done to learn how to utilize custom model binders to achieve this
goal.

## Highlights

- When a value type implements `IParseable<T>`, .NET automatically makes use of
  `static abstract TryParse` method in the interface. So no need to write custom
  model binders for custom value types any more :party:
- In case `IParseable` is not enough, `TypeConverter` classes is another option
  to bind a value type
- `record` types are allowed to have primary constructors, but regular `class`
  types with a primary constructor does not get automatically bound as a model

## Solution

We've used `IModelBinder` interface to implement a custom solution. It provides
a clean base to design a custom model binding for entities.

```csharp
public class CustomEntityBinder<T>(IGenericRepository<T> _repository)
    : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // ... put find entity logic here using `_repository`
    }
}
```

## Links to Follow

- [Model Binding][]
- [Custom Model Binding][]
- [Author Sample][]

[Model Binding]: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0
[Custom Model Binding]: https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-8.0
[Author Sample]: https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-8.0#custom-model-binder-sample
