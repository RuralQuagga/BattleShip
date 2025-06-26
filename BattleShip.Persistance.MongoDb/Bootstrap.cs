using BattleShip.Persistance.MongoDb.Configuration;
using BattleShip.Persistance.MongoDb.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BattleShip.Persistance.MongoDb
{
    public static class Bootstrap
    {
        public static IServiceCollection RegisterPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDBSettings:ConnectionString").Value!;
            var databaseName = configuration.GetSection("MongoDBSettings:DatabaseName").Value!;

            services.AddDbContext<BattleShipDbContext>(options =>
            {
                options.UseMongoDB(connectionString, databaseName);
            });            

            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

            return services;
        }
    }
}
