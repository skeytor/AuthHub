using AuthHub.Infrastructure.Authentication;
using AuthHub.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Testcontainers.MsSql;

namespace AuthHub.Api.IntegrationTest;

// Program.cs is internal default
/// <summary>
/// A custom <see cref="IntegrationTestWebApplicationFactory{TProgram}"/> for integration testing,
/// using a SQL Server container to provide a database enviroment.
/// Implements <see cref="IAsyncLifetime"/> to manage container lifecycle during the test.
/// </summary>
/// <typeparam name="TProgram">The entry point of the web application (usually the Program class).</typeparam>
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
        });
    }
    public Task InitializeAsync() => _msSqlContainer.StartAsync();

    public new Task DisposeAsync() => _msSqlContainer.StopAsync();
}
