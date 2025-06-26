using BattleShip.Application.Gameplay.Abstractions;

namespace BattleShip.API.Endpoints.Session
{
    internal static class StartNewSessionEndpoint
    {
        internal static async Task<IResult> ExecuteAsync(
            IFieldGameplayService service,
            CancellationToken cancellationToken)
        {
            var result = await service.StartNewSession(cancellationToken);

            return Results.Ok(result);
        }
    }
}
