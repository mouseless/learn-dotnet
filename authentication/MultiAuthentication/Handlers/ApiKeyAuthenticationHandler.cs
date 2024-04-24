using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MultiAuthentication.Handlers;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Headers.TryGetValue("X-API-KEY", out StringValues key))
        {
            var claim = new Claim("Token", $"{key}");
            var principal = new ClaimsPrincipal(new ClaimsIdentity([claim], "ApiKey"));

            return Task.FromResult(AuthenticateResult.Success(new(principal, "ApiKey")));
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
