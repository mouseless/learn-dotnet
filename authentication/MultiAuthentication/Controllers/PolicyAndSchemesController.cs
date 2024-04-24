using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MultiAuthentication.Controllers;

[ApiController]
[Route("authentication")]
public class PolicyAndSchemesController(IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [Route("default-scheme")]
    public object DefaultScheme()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToDictionary();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "OrganizationId")]
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
    [Route("backend-policy")]
    public object BackendPolicy()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToDictionary();
    }

    [HttpGet]
    [Authorize(Policy = "ExternalSystem")]
    [Produces("application/json")]
    [Route("external-system-policy")]
    public object ExternalSystemPolicy()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToDictionary();
    }
}
