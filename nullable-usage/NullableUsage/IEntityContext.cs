namespace NullableUsage;

public interface IEntityContext<T> where T : class
{
    T Insert(T enity);
    void Delete(T entity);
}

public class EntityContext<T> : IEntityContext<T> where T : class
{
    readonly AppDbContext _appDbContext;

    public EntityContext(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Delete(T entity)
    {
        _appDbContext.Set<T>().Remove(entity);

        _appDbContext.SaveChanges();
    }

    public T Insert(T entity)
    {
        var result = _appDbContext.Set<T>().Add(entity);

        _appDbContext.SaveChanges();

        return result.Entity;
    }
}
