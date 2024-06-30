using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Persistence.UnitTest.Context;

public static class ContextFactory
{
    public static AppDbContext CreateDatabaseInMemory()
    {
        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        return new AppDbContext(optionsBuilder.Options);
    }
    public static AppDbContext CreateSQLiteDatabaseInMemory()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        AppDbContext context = new(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }
}
