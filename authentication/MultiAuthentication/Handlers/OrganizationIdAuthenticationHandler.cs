using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MultiAuthentication.Handlers;

public class OrganizationIdAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Headers.TryGetValue("X-ORGANIZATION-ID", out StringValues key))
        {
            var claim = new Claim("OrganizationId", $"{key}");
            var principal = new ClaimsPrincipal(new ClaimsIdentity([claim], "OrganizationId"));

            return Task.FromResult(AuthenticateResult.Success(new(principal, "OrganizationId")));
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
