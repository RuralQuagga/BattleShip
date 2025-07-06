using BattleShip.Application.Gameplay.Abstractions;

namespace BattleShip.API.Endpoints.Session;

internal static class DeleteAllStatisticEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        await service.ClearStatistic(cancellationToken);

        return Results.Ok();
    }
}
