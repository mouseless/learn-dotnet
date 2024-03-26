namespace ModelBinders;

public class ModelOne(Guid _id, string _name)
{
    public Guid Id => _id;
    public string Name => _name;
}

public class ModelOnes : Dictionary<Guid, ModelOne>, IQuery<ModelOne>
{
}
