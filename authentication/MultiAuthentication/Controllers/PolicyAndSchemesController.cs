using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MultiAuthentication.Controllers;

[Authorize]
[ApiController]
[Route("authentication")]
public class PolicyAndSchemesController(IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("anonymous")]
    public object Anonymous()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToIdentityList();
    }

    [HttpGet]
    [Produces("application/json")]
    [Route("default")]
    public object Default()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToIdentityList();
    }

    [HttpGet]
    [Authorize(Policy = "RequireClaim")]
    [Produces("application/json")]
    [Route("default/claim")]
    public object DefaultWithClaim()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToIdentityList();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Alternative")]
    [Produces("application/json")]
    [Route("alternative")]
    public object Alternative()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToIdentityList();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Alternative")]
    [Authorize(Policy = "RequireClaim")]
    [Produces("application/json")]
    [Route("alternative/claim")]
    public object AlternativeWithClaim()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToIdentityList();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Alternative")]
    [Authorize(AuthenticationSchemes = "Default")]
    [Produces("application/json")]
    [Route("any")]
    public object Any()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToIdentityList();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Alternative")]
    [Authorize(AuthenticationSchemes = "Default")]
    [Authorize(Policy = "RequireClaim")]
    [Produces("application/json")]
    [Route("any/claim")]
    public object AnyWithClaim()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToIdentityList();
    }
}
