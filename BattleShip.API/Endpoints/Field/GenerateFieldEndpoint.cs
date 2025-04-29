using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Common.Enums;
using BattleShip.Common.Helpers;

namespace BattleShip.API.Endpoints.Field;

internal static class GenerateFieldEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        IFieldGameplayService service)
    {
        var result = await service.GenerateBattleField();

        return Results.Ok(result.ToIntMatrix());
    }
}
