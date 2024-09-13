using AuthHub.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AuthHub.Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddAuthenticationProvider(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, TokenProvider>();
        return services;
    }
}
