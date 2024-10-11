using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthHub.Api.Extensions;

/// <summary>
/// Provides extensions methods for applying database migrations in the application.
/// </summary>
internal static class MigrationExtensions
{
    /// <summary>
    /// Applies any pending database migrations during application startup.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance on the which to apply the migrations.</param>
    /// <remarks>
    /// This method creates a service scope to resolve the <see cref="AppDbContext"/>, then applies any
    /// pending migrations to ensure the database schema is up to date with the current model.
    /// </remarks>
    internal static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        AppDbContext dbContext = scope
            .ServiceProvider
            .GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}
