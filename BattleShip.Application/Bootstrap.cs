using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Gameplay.Services;
using BattleShip.Persistance.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BattleShip.Application;

public static class Bootstrap
{
    public static IServiceCollection AddBattleFieldApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterPersistance(configuration);
        services.AddScoped<IFieldGameplayService, FieldGameplayService>();

        return services;
    }
}
