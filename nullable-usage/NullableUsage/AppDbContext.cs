using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace NullableUsage;

public class AppDbContext : DbContext
{
    public DbSet<Person> Persons = default!;
    public static readonly MaterializationInterceptor MaterializationInterceptor = new();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(MaterializationInterceptor);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>().HasKey(p => p.Id);
    }
}

public class MaterializationInterceptor : IMaterializationInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        if (entity is Person person)
        {
            var fields = typeof(Person).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var dependency = fields.FirstOrDefault(f => f.FieldType == typeof(IEntityContext<Person>));

            if (dependency is not null)
            {
                dependency.SetValue(person, materializationData.Context.GetService<IEntityContext<Person>>());
            }
        }

        return entity;
    }
}