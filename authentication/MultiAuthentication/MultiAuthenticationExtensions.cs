using Microsoft.AspNetCore.Authentication;
using MultiAuthentication.Handlers;
using System.Security.Claims;

namespace MultiAuthentication;

public static class MultiAuthenticationExtensions
{
    public static void AddAuthenticationWithSchemeSelector(this IServiceCollection source)
    {
        source.AddAuthentication(options =>
        {
            options.DefaultScheme = "Default";
            options.DefaultAuthenticateScheme = "Default";
            options.AddScheme<DefaultAuthenticationHandler>("Default", default);
            options.AddScheme<AlternativeAuthenticationHandler>("Alternative", default);
            options.AddScheme<AnonymousAuthenticationHandler>("Anonymous", default);
        });

        source.Configure<AuthenticationSchemeOptions>("MultiAuthentication", options =>
            options.ForwardDefaultSelector = context =>
            {
                if (context.Request.Headers.ContainsKey("X-Default"))
                {
                    return "Default";
                }

                if (context.Request.Headers.ContainsKey("X-Alternative"))
                {
                    return "Alternative";
                }

                return "Anonymous";
            });

        source.AddOptions<AuthenticationSchemeOptions>();
        source.AddAuthorization(options =>
        {
            options.AddPolicy("RequireClaim", policy => policy.RequireClaim("Claim"));
        });
    }

    public static IEnumerable<Identity> ToIdentityList(this ClaimsPrincipal source)
    {
        foreach (var identity in source.Identities)
        {
            yield return new(identity);
        }
    }

    public record Identity(string Name, IEnumerable<Claim> Claims)
    {
        public Identity(ClaimsIdentity identity)
            : this(identity.AuthenticationType ?? "Anonymous", identity.Claims.Select(c => new Claim(c.Type, c.Value)))
        { }
    }

    public record Claim(string Type, string Value);
}