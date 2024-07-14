using AuthHub.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AuthHub.Api.IntegrationTest;

// Program.cs is default internal
internal class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            string? connString = GetConnectionString();
            services.AddSqlServer<AppDbContext>(connString);
            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
        });
    }
    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationTestWebApplicationFactory>()
            .Build();
        string? connString = configuration.GetConnectionString("AuthHub");
        return connString;
    }
    private static AppDbContext CreateDbContext(IServiceCollection services)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return dbContext;
    }
}
