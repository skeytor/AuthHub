namespace AuthHub.Api.IntegrationTest;

/// <summary>
/// Provides a base class for web application integration tests using an
/// <see cref="IntegrationTestWebApplicationFactory{TProgram}"/> to configure the test enviroment.
/// </summary>
/// <param name="factory"></param>
/// The <see cref="IntegrationTestWebApplicationFactory{TProgram}"/> used to create the test client
/// and configure the web application enviroment.
public abstract class BaseWebApplicationTest(IntegrationTestWebApplicationFactory<Program> factory)
{
    protected readonly HttpClient _httpClient = factory.CreateClient();
}