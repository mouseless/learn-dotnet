using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle;

public class DocumentBasedSecurityDefinition(string document, string _schemeId, OpenApiSecurityScheme _scheme)
    : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (context.DocumentName != document) { return; }

        swaggerDoc.Components ??= new OpenApiComponents();
        swaggerDoc.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        swaggerDoc.Components.SecuritySchemes[_schemeId] = _scheme;
    }
}