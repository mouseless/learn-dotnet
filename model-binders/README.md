# Model Binders

We needed to be able to use an entity directly from an action parameter. This
research was done to learn how to utilize custom model binders to achieve this
goal.

## Highlights

- When a value type implements `IParseable<T>`, .NET automatically makes use of
  `static abstract TryParse` method in the interface. So no need to write custom
  model binders for custom value types any more :party:
- ...

## Links to Follow

- [Model Binding][]
- [Custom Model Binding][]

[Model Binding]: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0
[Custom Model Binding]: https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-8.0
