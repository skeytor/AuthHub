using AuthHub.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.IntegrationTest.Helpers;

internal static class TestHelper
{

    public static AppDbContext GetDbContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString);
        return new AppDbContext(optionsBuilder.Options);
    }
}
