using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandling;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ParameterRequiredException parameterRequiredException) { return false; }

        var problemDetails = new ProblemDetails
        {
            Type = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/errors/parameter-required",
            Status = httpContext.Response.StatusCode,
            Title = "Parameter Required",
            Detail = parameterRequiredException.Message,
            Extensions = new Dictionary<string, object?>
            {
                { "name", parameterRequiredException.Name }
            }
        };

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}