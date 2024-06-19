using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthHub.UnitTests.Helpers;

public static class AppDbContextGeneratorTest
{
    public static AppDbContext Generate()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        return new AppDbContext(optionsBuilder.Options);
    }
}
