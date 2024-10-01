using AuthHub.Api.Extensions;
using AuthHub.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add app repositories to the container.
builder.Services.AddRepositories(builder.Configuration);

// Add application services to the container.
builder.Services.AddAppServices();

// Add a custom authorization and authentication configuration to the container.
builder.Services.AddAuthenticationAndAuthorization();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.EnablePersistAuthorization();
    });
    app.ApplyMigrations();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }