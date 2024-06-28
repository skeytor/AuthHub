using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthHub.UnitTests.Context;

public static class AppDbContextFactoryTest
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
/*
public class AppDbContextFixture : IDisposable
{
    public AppDbContext Context { get; private set; }
    public AppDbContextFixture()
    {
        Context = CreateSQLiteDatabaseInMemory();
    }
    private static AppDbContext CreateSQLiteDatabaseInMemory()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        AppDbContext context = new(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }


    public void Dispose()
    {
        Context.Database.CloseConnection();
        Context.Dispose();
    }
}*/