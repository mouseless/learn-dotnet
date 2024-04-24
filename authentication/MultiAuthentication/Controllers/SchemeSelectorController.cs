using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MultiAuthentication.Controllers;

[ApiController]
[Route("authentication")]
public class SchemeSelectorController(IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [Route("scheme-selector")]
    public object SchemeSelector()
    {
        var principal = httpContextAccessor.HttpContext?.User ?? throw new();

        return principal.ToDictionary();
    }
}
