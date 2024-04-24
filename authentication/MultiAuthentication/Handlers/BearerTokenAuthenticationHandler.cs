using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MultiAuthentication.Handlers;

public class BearerTokenAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Context.Request.Headers.Authorization.Any())
        {
            var givenToken = $"{Context.Request.Headers.Authorization}".Replace("Bearer", string.Empty).Trim();

            var claim = new Claim("Token", $"{givenToken}");
            var principal = new ClaimsPrincipal(new ClaimsIdentity([claim], "BearerToken"));

            return Task.FromResult(AuthenticateResult.Success(new(principal, "BearerToken")));
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
