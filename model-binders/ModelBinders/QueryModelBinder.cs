using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ModelBinders;

public class QueryModelBinder<TModel>(IQuery<TModel> _query)
  : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        // Try to fetch the value of the argument by name
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(value)) { return Task.CompletedTask; }

        if (!Guid.TryParse(value, out var id))
        {
            bindingContext.ModelState.TryAddModelError(modelName, "Model Id must be a guid.");

            return Task.CompletedTask;
        }

        if (!_query.TryGetValue(id, out var model))
        {
            bindingContext.ModelState.TryAddModelError(modelName, $"Model not found with id {id}");

            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(model);

        return Task.CompletedTask;
    }
}
