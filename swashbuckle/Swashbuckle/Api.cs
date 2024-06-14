using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle;

[ApiController]
public class Api
{
    [HttpGet("/resources")]
    [ApiExplorerSettings(GroupName = "Resources")]
    [Document("api")]
    public void GetResources() { }

    public record PostResourceRequest(string Name);

    [HttpPost("/resources")]
    [ApiExplorerSettings(GroupName = "Resources")]
    [Document("api")]
    public void PostResource([FromBody] PostResourceRequest _) { }

    [HttpGet("/internal")]
    [Internal]
    public void InternalAction() { }

    [HttpGet("/admin/resources")]
    [ApiExplorerSettings(GroupName = "Resources")]
    [Document("admin")]
    public void GetAdminResources() { }
}