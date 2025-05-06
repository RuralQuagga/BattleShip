using BattleShip.Common.Enums;

namespace BattleShip.Application.Gameplay.Abstractions;

public interface IFieldGameplayService
{
    Task<CellType[][]> GenerateBattleField();

    Task<string> StartNewSession(CancellationToken cancellationToken);

    Task<string> ChangeSessionStateToInProgress(string sessionId, CancellationToken cancellationToken);
}
