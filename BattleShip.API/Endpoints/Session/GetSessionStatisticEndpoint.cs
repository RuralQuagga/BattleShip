using BattleShip.Application.Gameplay.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.API.Endpoints.Session;

internal static class GetSessionStatisticEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        [FromQuery]string sessionId,
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetSessionStatistic(sessionId, cancellationToken);

        return Results.Ok(result);
    }
}
