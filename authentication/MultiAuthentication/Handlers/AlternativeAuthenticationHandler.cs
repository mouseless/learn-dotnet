using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MultiAuthentication.Handlers;

public class AlternativeAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.TryGetValue("X-Alternative", out var value))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (value.ToString() != "Alternative")
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid token value"));
        }

        var principal = new ClaimsPrincipal(new ClaimsIdentity("Alternative"));
        if (Context.Request.Headers.TryGetValue("X-Claim", out var claim))
        {
            ((ClaimsIdentity?)principal.Identity)?.AddClaim(new($"{claim}", $"{claim}"));
        }

        return Task.FromResult(AuthenticateResult.Success(new(principal, "Alternative")));
    }
}