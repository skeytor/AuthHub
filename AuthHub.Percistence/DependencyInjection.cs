using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthHub.Persistence
{
    public static class DependencyInjection
    {
        private const string _connectionName = "Default";
        public static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            IConfiguration configuration) 
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(_connectionName)));

            return services;
        }
    }
}
