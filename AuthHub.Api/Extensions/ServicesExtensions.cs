using AuthHub.Api.Services.UserService;
using AuthHub.Persistence.Abstractions;

namespace AuthHub.Api.Extensions;

internal static class ServicesExtensions
{
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
