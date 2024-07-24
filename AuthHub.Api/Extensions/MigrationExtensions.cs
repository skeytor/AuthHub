using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthHub.Api.Extensions;

internal static class MigrationExtensions
{
    /// <summary>
    /// Apply migrations
    /// </summary>
    /// <param name="app"></param>
    internal static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope
            .ServiceProvider
            .GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}
