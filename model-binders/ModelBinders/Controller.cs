using Microsoft.AspNetCore.Mvc;

namespace ModelBinders;

[ApiController]
public class Controller
{
    [HttpGet]
    [Route("model-ones/{model-id}")]
    public ModelOne GetModelOne([FromRoute(Name = "model-id")] ModelOne model) =>
        model;

    [HttpPost]
    [Route("model-ones")]
    public ModelOne Post([FromServices] IQuery<ModelOne> modelOnes, string name)
    {
        var id = Guid.NewGuid();

        modelOnes.Add(id, new(id, name));

        return modelOnes[id];
    }

    [HttpGet]
    [Route("model-twos/{model-id}")]
    public ModelTwo GetModelTwo([FromRoute(Name = "model-id")] ModelTwo model) =>
        model;

    [HttpPost]
    [Route("model-twos")]
    public ModelTwo Post([FromServices] IQuery<ModelTwo> modelTwos, string name)
    {
        var id = Guid.NewGuid();

        modelTwos.Add(id, new(id, name));

        return modelTwos[id];
    }
}
