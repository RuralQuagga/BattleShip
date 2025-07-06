using BattleShip.Application.Gameplay.Abstractions;

namespace BattleShip.API.Endpoints.Session;

internal static class GetFullStatisticEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetFullStatistic(cancellationToken);

        return Results.Ok(result);
    }
}
