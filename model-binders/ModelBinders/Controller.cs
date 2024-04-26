using Microsoft.AspNetCore.Mvc;

namespace ModelBinders;

[ApiController]
public class Controller
{
    [HttpGet]
    [Route("model-ones/{modelId}")]
    public ModelOne GetModelOne([FromRoute(Name = "modelId")] ModelOne model) =>
        model;

    public record PostModelOneRequest(string Name);

    [HttpPost]
    [Route("model-ones")]
    public ModelOne PostModelOne([FromServices] IQuery<ModelOne> modelOnes, [FromBody] PostModelOneRequest request)
    {
        var id = Guid.NewGuid();

        modelOnes.Add(id, new(id, request.Name));

        return modelOnes[id];
    }

    [HttpGet]
    [Route("model-twos")]
    public ModelTwo GetModelTwo([FromQuery(Name = "modelId")] ModelTwo model) =>
        model;

    public record PostModelTwoRequest(string Name);

    [HttpPost]
    [Route("model-twos")]
    public ModelTwo Post([FromServices] IQuery<ModelTwo> modelTwos, [FromBody] PostModelTwoRequest request)
    {
        var id = Guid.NewGuid();

        modelTwos.Add(id, new(id, request.Name));

        return modelTwos[id];
    }
}