using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Gameplay.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BattleShip.Application;

public static class Bootstrap
{
    public static IServiceCollection AddBattleFieldApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFieldGameplayService, FieldGameplayService>();

        return services;
    }
}
