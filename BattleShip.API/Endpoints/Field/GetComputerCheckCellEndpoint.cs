using BattleShip.Application.Gameplay.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.API.Endpoints.Field;

internal static class GetComputerCheckCellEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        [FromQuery] string fieldId,
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetComputerMove(fieldId, cancellationToken);

        return Results.Ok(result);
    }
}
