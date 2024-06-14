using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle;

public class AddParameterToPostOperations(ParameterLocation @in, string name)
  : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.HttpMethod != "POST") { return; }

        operation.Parameters.Insert(0, new()
        {
            In = @in,
            Name = name,
        });
    }
}