using AuthHub.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace AuthHub.Api.IntegrationTest;

// Program.cs is internal default
public class IntegrationTestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.AddSqlServer<AppDbContext>(_msSqlContainer.GetConnectionString());
            //var dbContext = CreateDbContext(services);
            //dbContext.Database.EnsureDeleted();
        });
    }
    /*private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationTestWebApplicationFactory<TProgram>>()
            .Build();
        string? connString = configuration.GetConnectionString("AuthHub");
        return connString;
    }*/
    /*private static AppDbContext CreateDbContext(IServiceCollection services)
    {
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        IServiceScope scope = serviceProvider.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return dbContext;
    }*/

    public Task InitializeAsync() => _msSqlContainer.StartAsync();

    public new Task DisposeAsync() => _msSqlContainer.StopAsync();
}
