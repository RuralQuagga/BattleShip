using BattleShip.API.Contracts;
using BattleShip.API.Mappers.Field;
using BattleShip.Application.Gameplay.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.API.Endpoints.Field;

internal static class CheckCellEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        [FromBody] CheckCellApiRequest request,
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        var result = await service.CheckCell(request.ToDto(), cancellationToken);

        return Results.Ok(result);
    }
}
