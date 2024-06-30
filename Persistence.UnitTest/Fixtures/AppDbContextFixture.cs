using AuthHub.Persistence;
using AuthHub.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitTest.Context;

namespace Persistence.UnitTest.Fixtures;

public class AppDbContextFixture
{
    public readonly IAppDbContext context;
    public readonly IUnitOfWork unitOfWork;

    public AppDbContextFixture()
    {
        context = ContextFactory.CreateSQLiteDatabaseInMemory();
        unitOfWork = (IUnitOfWork)context;
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
}
*/