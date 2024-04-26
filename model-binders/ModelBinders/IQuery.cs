namespace ModelBinders;

public interface IQuery<TModel> : IDictionary<Guid, TModel>
{
}