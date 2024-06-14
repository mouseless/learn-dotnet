using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle;

public class AddParameterToPostOperations(ParameterLocation _in, string _name)
  : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.HttpMethod != "POST") { return; }

        operation.Parameters.Insert(0, new()
        {
            In = _in,
            Name = _name,
        });
    }
}