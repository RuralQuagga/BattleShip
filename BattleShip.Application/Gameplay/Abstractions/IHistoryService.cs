using BattleShip.Persistance.MongoDb.Entities;

namespace BattleShip.Application.Gameplay.Abstractions;

public interface IHistoryService
{
    Task AddRecordToHistory(string fieldId,
                                         string sessionId,
                                         string? shipId,
                                         bool IsPlayerAction,
                                         int lineIndex,
                                         int cellIndex,
                                         CancellationToken cancellationToken);

    Task<bool> CheckIsShipDead(ShipEntity ship, CancellationToken cancellationToken);

    Task<IEnumerable<GameHistory>> GetFieldHistory(string fieldId, CancellationToken cancellationToken);
}
