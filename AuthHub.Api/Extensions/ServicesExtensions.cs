﻿using AuthHub.Api.Services.Users;
using AuthHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthHub.Api.Extensions;

internal static class ServicesExtensions
{
    internal static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        return services;
    }
}
