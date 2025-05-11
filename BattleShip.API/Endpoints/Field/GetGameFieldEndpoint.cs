using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.API.Endpoints.Field;

internal static class GetGameFieldEndpoint
{
    internal static async Task<IResult> ExecuteAsync(
        [FromQuery]string sessionId,
        [FromQuery]FieldType fieldType,
        IFieldGameplayService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetGameField(sessionId, fieldType, cancellationToken);

        return Results.Ok(result);
    }
}
