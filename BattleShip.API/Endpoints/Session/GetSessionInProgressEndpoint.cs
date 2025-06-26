using BattleShip.Application.Gameplay.Abstractions;

namespace BattleShip.API.Endpoints.Session;

internal static class GetSessionInProgressEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        var result = await service.CheckSessionInProgress(cancellationToken);

        return Results.Ok(result);
    }
}
