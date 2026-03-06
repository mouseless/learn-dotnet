using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle;

public class AddParameterToPostOperations(ParameterLocation _in, string _name)
  : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.HttpMethod != "POST") { return; }

        operation.Parameters ??= [];
        operation.Parameters.Insert(0, new OpenApiParameter
        {
            In = _in,
            Name = _name,
        });
    }
}