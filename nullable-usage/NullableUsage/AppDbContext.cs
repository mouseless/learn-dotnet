using Microsoft.EntityFrameworkCore;

namespace NullableUsage;

public class AppDbContext : DbContext
{
    public DbSet<Person> Persons = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>().HasKey(p => p.Id);
    }
}
