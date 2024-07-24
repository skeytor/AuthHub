using AuthHub.Persistence;

namespace AuthHub.Api.IntegrationTest;

public abstract class BaseWebApplicationTest(IntegrationTestWebApplicationFactory<Program> factory)
{
    protected readonly HttpClient _httpClient = factory.CreateClient();
    protected readonly AppDbContext _context = factory.Context!;
}