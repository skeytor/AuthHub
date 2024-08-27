using AuthHub.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace AuthHub.Api.IntegrationTest;

public abstract class BaseWebApplicationTest
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _httpClient;
    protected readonly AppDbContext _context;

    protected BaseWebApplicationTest(IntegrationTestWebApplicationFactory<Program> factory)
    {
        _scope = factory.Services.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _httpClient = factory.CreateClient();
    }
}