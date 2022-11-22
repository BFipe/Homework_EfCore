using Microsoft.EntityFrameworkCore;

namespace Homework_EfCore.Database
{
    public static class UseDBService
    {
        public static IServiceCollection AddEfCoreDataManagement(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyDBContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IDBService, DBService>();

            return services;
        }
    }
}
