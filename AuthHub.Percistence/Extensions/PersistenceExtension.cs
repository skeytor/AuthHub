﻿using AuthHub.Domain.Repositories;
using AuthHub.Persistence.Abstractions;
using AuthHub.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthHub.Persistence.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Database")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAppDbContext>(options => options.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork>(options => options.GetRequiredService<AppDbContext>());
        return services;
    }
}