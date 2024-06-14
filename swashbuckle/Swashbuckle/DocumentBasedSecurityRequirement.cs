using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle;

public class DocumentBasedSecurityRequirement(string _document, string _schemeId)
    : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.DocumentName != _document) { return; }

        operation.Security.Add(new()
        {
            {
                new() { Reference = new() { Type = ReferenceType.SecurityScheme, Id = _schemeId } },
                Array.Empty<string>()
            }
        });
    }
}