using AuthHub.Api.OptionsSetup;
using AuthHub.Api.Services.Auth;
using AuthHub.Api.Services.Permissions;
using AuthHub.Api.Services.Roles;
using AuthHub.Api.Services.Users;
using AuthHub.Domain.Entities;
using AuthHub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.Extensions;
/// <summary>
/// Provides extension methods for adding services.
/// </summary>
internal static class ServicesExtensions
{
    /// <summary>
    /// Registers the application's core services to the specified <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the services are added.</param>
    /// <returns>The <see cref="IServiceCollection"/> with the registered services.</returns>
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        return services;
    }
    /// <summary>
    /// Configures JWT-based authentication and adds authorization services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which authentication and authorization services are added.</param>
    /// <returns>The <see cref="IServiceCollection"/> with the registered authentication and authorization services.</returns>
    /// <remarks>
    /// This method configures JWT Bearer authentication using the default JWT Bearer scheme and sets up authorization policies.
    /// It also configures custom JWT options using <see cref="JwtBearerConfigureOptions"/> and <see cref="JwtBearerParametersConfigureOptions"/>.
    /// </remarks>
    internal static IServiceCollection AddAuthenticationAndAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        // Add JWT Provider from Infrastructure Project
        services.AddJwtBearerTokenProvider();

        // Add custom policy provider from Infrastructure project
        services.AddAuthorizationPolicyProvider();

        services
            .ConfigureOptions<JwtBearerConfigureOptions>()
            .ConfigureOptions<JwtBearerParametersConfigureOptions>();
        return services;
    }
    /// <summary>
    /// Adds and configures Swagger for API documentation generation.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which Swagger services are added.</param>
    /// <returns>The <see cref="IServiceCollection"/> with the registered Swagger services.</returns>
    /// <remarks>
    /// This method configures Swagger using <see cref="SwaggerGen"/> and custom options
    /// provided by <see cref="ConfigureSwaggerGenOptions"/>.
    /// </remarks>
    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        return services;
    }
}
