using Testcontainers.MsSql;

namespace App.Persistence.IntegrationTest;


public class MsSqlContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();
    public string ConnectionString { get => _msSqlContainer.GetConnectionString(); }
    public Task DisposeAsync() => _msSqlContainer.DisposeAsync().AsTask();

    public Task InitializeAsync() => _msSqlContainer.StartAsync();
}
