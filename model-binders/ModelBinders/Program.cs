using ModelBinders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new QueryModelBinderProvider());
});
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<Models>();
builder.Services.AddSingleton<IQuery<Model>>(sp => sp.GetRequiredService<Models>());
builder.Services.AddSingleton(typeof(QueryModelBinder<>));

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
