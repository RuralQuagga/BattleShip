using BattleShip.Application.Gameplay.Abstractions;

namespace BattleShip.API.Endpoints.Session
{
    internal static class StartNewSessionEndpoint
    {
        internal static async Task<IResult> ExecuteAsync(
            IFieldGameplayService service,
            CancellationToken cancellationToken)
        {
            var sessionId = await service.StartNewSession(cancellationToken);

            return Results.Ok(sessionId);
        }
    }
}
