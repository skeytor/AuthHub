using AuthHub.Persistence;
using AuthHub.Persistence.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AuthHub.Api.IntegrationTest;

// Program.cs is default internal
///
public class IntegrationTestWebApplicationFactory<TProgram> 
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public IAppDbContext? Context { get; private set; }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            string? connString = GetConnectionString();
            services.AddSqlServer<AppDbContext>(connString);
            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();
            Context = dbContext;
        });
    }
    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationTestWebApplicationFactory<TProgram>>()
            .Build();
        string? connString = configuration.GetConnectionString("AuthHub");
        return connString;
    }
    private static AppDbContext CreateDbContext(IServiceCollection services)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        IServiceScope scope = serviceProvider.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return dbContext;
    }
}
