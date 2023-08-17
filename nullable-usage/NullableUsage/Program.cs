using Microsoft.EntityFrameworkCore;
using NullableUsage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PersonService>();
builder.Services.AddSingleton(typeof(Func<Person>), (sp) => () => sp.GetRequiredService<Person>());
builder.Services.AddTransient<Person>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("nullable-usage.db"));

builder.Services.AddTransient(typeof(IEntityContext<>), typeof(EntityContext<>));
builder.Services.AddScoped(typeof(IQueryContext<>), typeof(QueryContext<>));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
