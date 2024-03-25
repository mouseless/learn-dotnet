namespace ModelBinders;

public class ModelTwo(Guid _id, string _name)
{
    public Guid Id => _id;
    public string Name => _name;
}

public class ModelTwos : Dictionary<Guid, ModelTwo>, IQuery<ModelTwo>
{
}
