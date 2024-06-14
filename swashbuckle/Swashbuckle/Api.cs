using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle;

[ApiController]
public class Api
{
    [HttpGet("/endpoint")]
    public void Endpoint() { }
}