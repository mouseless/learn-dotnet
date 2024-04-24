using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MultiAuthentication.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationSamplesController(IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = "ApiKey,BearerToken")]
    public string Get()
    {
        var identites = httpContextAccessor.HttpContext?.User.Identities ?? throw new();
        var result = new Dictionary<string, List<string>>();
        foreach (var identity in identites)
        {
            result.Add(identity.AuthenticationType ?? throw new(), identity.Claims.Select(c => c.Type).ToList());
        }

        return JsonSerializer.Serialize(result);
    }
}
