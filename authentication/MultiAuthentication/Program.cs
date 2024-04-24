using Microsoft.AspNetCore.Authentication;
using MultiAuthentication.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", _ => { })
    .AddScheme<AuthenticationSchemeOptions, BearerTokenAuthenticationHandler>("BearerToken", _ => { });
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
#pragma warning disable IDE0058 // Expression value is never used
    app.UseSwagger();
    app.UseSwaggerUI();
#pragma warning restore IDE0058 // Expression value is never used
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
