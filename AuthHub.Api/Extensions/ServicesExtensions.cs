using AuthHub.Api.OptionsSetup;
using AuthHub.Api.Services.Auth;
using AuthHub.Api.Services.Roles;
using AuthHub.Api.Services.Users;
using AuthHub.Domain.Entities;
using AuthHub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.Extensions;

internal static class ServicesExtensions
{
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        return services;
    }
    /// <summary>
    /// Configure Authentication options and add Authorization for the application.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    internal static IServiceCollection AddAuthenticationAndAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        // Add JWT Provider from Infrastructure Project
        services.AddJwtBearerTokenProvider();
        services
            .ConfigureOptions<JwtBearerConfigureOptions>()
            .ConfigureOptions<JwtBearerParametersConfigureOptions>();
        return services;
    }
    
    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        return services;
    }
}
