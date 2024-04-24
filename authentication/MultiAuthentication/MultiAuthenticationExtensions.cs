using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using MultiAuthentication.Controllers;
using MultiAuthentication.Handlers;
using System.Reflection;
using System.Security.Claims;

namespace MultiAuthentication;

public static class MultiAuthenticationExtensions
{
    public static void AddAuthenticationWithSchemeSelector(this IServiceCollection source)
    {
        source.AddMvcCore().ConfigureApplicationPartManager(manager =>
        {
            manager.ApplicationParts.Clear();
            manager.ApplicationParts.Add(new SingleControllerApplicationPart<SchemeSelectorController>());
        });

        source.AddAuthentication(options =>
        {
            options.DefaultScheme = "ApiKey";
            options.DefaultAuthenticateScheme = "ApiKey";
            options.AddScheme<ApiKeyAuthenticationHandler>("ApiKey", "ApiKey");
            options.AddScheme<BearerTokenAuthenticationHandler>("BearerToken", "BearerToken");
            options.AddScheme<OrganizationIdAuthenticationHandler>("OrganizationId", "OrganizationId");

        });

        source.Configure<AuthenticationSchemeOptions>("ApiKey", options =>
            options.ForwardDefaultSelector = context =>
            {
                if (context.Request.Headers.ContainsKey("X-ORGANIZATION-ID"))
                {
                    return "OrganizationId";
                }

                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    return "BearerToken";
                }

                return "ApiKey";
            });

        source.AddOptions<AuthenticationSchemeOptions>();
        source.AddAuthorization();
    }

    public static void AddAuthenticationDefinedSchemesInAttribute(this IServiceCollection source)
    {
        source.AddMvcCore().AddApiExplorer().ConfigureApplicationPartManager(manager =>
        {
            manager.ApplicationParts.Clear();
            manager.ApplicationParts.Add(new SingleControllerApplicationPart<PolicyAndSchemesController>());
        });

        source.AddAuthentication("ApiKey")
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", _ => { })
            .AddScheme<AuthenticationSchemeOptions, BearerTokenAuthenticationHandler>("BearerToken", _ => { })
            .AddScheme<AuthenticationSchemeOptions, OrganizationIdAuthenticationHandler>("OrganizationId", _ => { });

        source.AddAuthorization(options =>
        {
            var backendPolicyBuilder = new AuthorizationPolicyBuilder(
                "ApiKey",
                "OrganizationId"
            ).RequireAuthenticatedUser();

            options.AddPolicy("Backend", backendPolicyBuilder.Build());

            var externalSystemPolicyBuilder = new AuthorizationPolicyBuilder(
                "BearerToken",
                "ApiKey"
            ).RequireAuthenticatedUser();

            options.AddPolicy("ExternalSystem", externalSystemPolicyBuilder.Build());
        });
    }

    public static Dictionary<string, Dictionary<string, string>> ToDictionary(this ClaimsPrincipal source)
    {
        var result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var identity in source.Identities)
        {
            result.Add(identity.AuthenticationType ?? throw new(), identity.Claims.ToDictionary(c => c.Type, c => c.Value));
        }

        return result;
    }

    class SingleControllerApplicationPart<T> : ApplicationPart, IApplicationPartTypeProvider
    where T : ControllerBase
    {
        public override string Name => typeof(T).Name;

        public IEnumerable<TypeInfo> Types => [typeof(T).GetTypeInfo()];
    }
}


