using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle;

public class DocumentBasedSecurityRequirement(string _document, string _schemeId)
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.DocumentName != _document) { return; }

        operation.Security ??= [];
        operation.Security.Add(new()
            {
                { new OpenApiSecuritySchemeReference(_schemeId), [] },
            }
        );
    }
}