namespace ModelBinders;

public class Model(Guid _id, string _name)
{
    public Guid Id => _id;
    public string Name => _name;
}

public class Models : Dictionary<Guid, Model>, IQuery<Model>
{
}
