﻿using AuthHub.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AuthHub.Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddJwtBearerTokenProvider(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, TokenProvider>();
        return services;
    }
    public static IServiceCollection AddAuthorizationPolicyProvider(this IServiceCollection services) 
    {
        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        return services;
    }
}