using DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddControllers().AddApplicationPart(typeof(Program).Assembly);
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Singleton>();

builder.Services.AddSingleton<Func<Scoped>>(sp => () => sp.GetRequiredServiceUsingRequestServices<Scoped>());
builder.Services.AddScoped<Scoped>();

builder.Services.AddSingleton<Func<Transient>>(sp => () => sp.GetRequiredServiceUsingRequestServices<Transient>());
builder.Services.AddTransient<Transient>();

var app = builder.Build();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");

    return Task.CompletedTask;
});

app.Run();
