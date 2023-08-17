namespace NullableUsage;

public interface IQueryContext<T> where T : class
{
    IEnumerable<T> All();

    T? SingleById(int id);
}

public class QueryContext<T> : IQueryContext<T> where T : class
{
    readonly AppDbContext _appDbContext;

    public QueryContext(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<T> All()
    {
        return _appDbContext.Set<T>();
    }

    public T? SingleById(int id)
    {
        return _appDbContext.Find<T>(id);
    }
}
