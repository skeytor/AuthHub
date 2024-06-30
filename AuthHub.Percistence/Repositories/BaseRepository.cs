using AuthHub.Persistence.Abstractions;

namespace AuthHub.Persistence.Repositories
{
    public abstract class BaseRepository(IAppDbContext context)
    {
        protected readonly IAppDbContext _Context = context;
    }
}
