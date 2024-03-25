using Microsoft.AspNetCore.Mvc;

namespace ModelBinders;

[ApiController]
public class Controller
{
    [HttpGet]
    [Route("bind/{model}")]
    public Model Get([FromRoute] Model model) =>
        model;

    [HttpPost]
    [Route("post")]
    public Model Post([FromServices] Models models, string name)
    {
        var id = Guid.NewGuid();

        models.Add(id, new(id, name));

        return models[id];
    }
}
