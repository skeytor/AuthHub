using AuthHub.Api.IntegrationTest.Initialization;
using AuthHub.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace AuthHub.Api.IntegrationTest;

/// <summary>
/// Provides a base class for web application integration tests, using an
/// <see cref="IntegrationTestWebApplicationFactory{TProgram}"/> to set up and configure the test enviroment.
/// </summary>
/// <remarks>
/// This class serves as a foundation for integration tests by providing a shared
/// <see cref="HttpClient"/> instance and initializing the test data. It uses the
/// <see cref="IntegrationTestWebApplicationFactory{TProgram}"/> used to create the test client
/// and handle services, including resetting the database with seed data before each test run.
/// </remarks>
/// <param name="factory"></param>
public abstract class BaseWebApplicationTest
{
    protected readonly HttpClient _httpClient;
    protected readonly IntegrationTestWebApplicationFactory<Program> _factory;
    protected readonly ITestOutputHelper _testOutputHelper;
    protected BaseWebApplicationTest(
        IntegrationTestWebApplicationFactory<Program> factory, 
        ITestOutputHelper testOutputHelper)
    {
        _httpClient = factory.CreateClient();
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        DataInitializer();
    }

    /// <summary>
    /// Resets the database and reseeds it with test data.
    /// </summary>
    /// <remarks>
    /// This method is called during class initialization to ensure a fresh 
    /// database state with known test data. It creates a new service scope, 
    /// retrieves the <see cref="AppDbContext"/>, and invokes the 
    /// <see cref="SampleDataInitializer"/> to clear and reseed the database.
    /// </remarks>
    private void DataInitializer()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        SampleDataInitializer.ClearAndReseedDatabase(context);
    }
}