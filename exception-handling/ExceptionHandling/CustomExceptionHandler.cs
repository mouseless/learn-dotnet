using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandling;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ParameterRequiredException actualException) { return false; }

        var problemDetails = new ProblemDetails
        {
            Type = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/errors/parameter-required",
            Status = StatusCodes.Status500InternalServerError,
            Title = "Parameter Required",
            Detail = actualException.Message,
            Extensions = new Dictionary<string, object?>
            {
                { "key1", "value1" },
                { "key2", 2 }
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
