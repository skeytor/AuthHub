using Microsoft.EntityFrameworkCore;

namespace AuthHub.Persistence
{
    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) { }
}
