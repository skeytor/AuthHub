using App.Persistence.IntegrationTest.Helpers;
using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit.Abstractions;

namespace App.Persistence.IntegrationTest;

public abstract class BaseTest: IClassFixture<MsSqlContainerFixture>, IDisposable
{
    protected readonly AppDbContext Context;
    protected readonly ITestOutputHelper OutputHelper;
    protected BaseTest(MsSqlContainerFixture fixture, ITestOutputHelper outputHelper)
    {
        Context = TestHelper.GetDbContext(fixture.ConnectionString);
        Context.Database.EnsureCreated();
        OutputHelper = outputHelper;
    }

    public virtual void Dispose() => Context.Dispose();

    protected void ExecuteInATransaction(Action actionToExecute)
    {
        IExecutionStrategy strategy = Context.Database.CreateExecutionStrategy();
        strategy.Execute(() =>
        {
            using IDbContextTransaction transacction = Context.Database.BeginTransaction();
            actionToExecute();
            transacction.Rollback();
        });
    }
}
