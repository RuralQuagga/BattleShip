using BattleShip.Application.Gameplay.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.API.Endpoints.Session;

internal static class StartPlayGameEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        [FromRoute] string sessionId,
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        var result = await service.ChangeSessionStateToInProgress(sessionId, cancellationToken);

        return Results.Ok(result);
    }
}
