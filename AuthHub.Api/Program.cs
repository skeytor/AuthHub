using AuthHub.Api.Extensions;
using AuthHub.Api.OptionsSetup;
using AuthHub.Infrastructure.Extensions;
using AuthHub.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add app repositories
builder.Services.AddRepositories(builder.Configuration);

// Add app services
builder.Services.AddAppServices();

builder.Services
    .ConfigureOptions<OptionsTokenSetup>();

builder.Services.AddAuthenticationProvider();
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }