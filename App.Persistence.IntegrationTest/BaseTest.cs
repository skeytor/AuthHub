using App.Persistence.IntegrationTest.Helpers;
using App.Persistence.IntegrationTest.Initialization;
using AuthHub.Persistence;

namespace App.Persistence.IntegrationTest;

public abstract class BaseTest : IClassFixture<MsSqlContainerFixture>
{
    protected readonly AppDbContext Context;

    protected BaseTest(MsSqlContainerFixture fixture)
    {
        Context = TestHelper.GetDbContext(fixture.ConnectionString);
        Context.Database.EnsureCreated();
        DataInitializer.ClearAndReseedDatabase(Context);
    }
}
