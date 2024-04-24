using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MultiAuthentication.Controllers;

[ApiController]
[Route("authentication")]
public class PolicyAndSchemesController(IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Default")]
    [Produces("application/json")]
    [Route("single-scheme")]
    public object SingleScheme()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToDictionary();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "ApiKey,BearerToken")]
    [Produces("application/json")]
    [Route("multi-authentication-scheme")]
    public object MultiAuthenticationScheme()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToDictionary();
    }

    [HttpGet]
    [Authorize(Policy = "Backend")]
    [Produces("application/json")]
    [Route("policy-with-multi-scheme")]
    public object MultiSchemeFromPolicy()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToDictionary();
    }
}
