using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Persistance.MongoDb.Entities;
using BattleShip.Persistance.MongoDb.Repository;
using MongoDB.Bson;

namespace BattleShip.Application.Gameplay.Services;

public class HistoryService(IRepository<GameHistory> historyRepository) : IHistoryService
{
    public async Task AddRecordToHistory(string fieldId,
                                         string sessionId,
                                         string? shipId,
                                         bool isPlayerAction,
                                         int lineIndex,
                                         int cellIndex,
                                         CancellationToken cancellationToken)
    {

        await historyRepository.AddAsync(
            new GameHistory
            {
                RecordId = ObjectId.GenerateNewId().ToString(),
                ActionPoint = new System.Drawing.Point(cellIndex, lineIndex),
                FieldId = fieldId,
                IsPlayerAction = isPlayerAction,
                SessionId = sessionId, 
                ShipId = shipId,
                IsSuccessAction = shipId is not null,
                TimeStamp = DateTime.Now
            }, cancellationToken);
    }

    public async Task<bool> CheckIsShipDead(ShipEntity ship, CancellationToken cancellationToken)
    {
        var historyRecords = await historyRepository.GetAllAsync(cancellationToken);

        var actionPointsFromHistoryForCurrentShip = historyRecords.Where(r => !string.IsNullOrEmpty(r.ShipId) && r.ShipId.Equals(ship.ShipId, StringComparison.OrdinalIgnoreCase))
            .Select(r => r.ActionPoint);

        return ship.Points.All(p => actionPointsFromHistoryForCurrentShip.Contains(p));
    }

    public async Task DeleteAll(CancellationToken cancellationToken)
    {
        await historyRepository.DeleteAll(cancellationToken);
    }

    public async Task<IEnumerable<GameHistory>> GetFieldHistory(string fieldId, CancellationToken cancellationToken)
    {
        var history = await historyRepository.GetAllAsync(cancellationToken);

        return history.Where(h => h.FieldId.Equals(fieldId, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<GameHistory>> GetSessionHistory(string sessionId, CancellationToken cancellationToken)
    {
        var history = await historyRepository.GetAllAsync(cancellationToken);

        return history.Where(h => h.SessionId.Equals(sessionId, StringComparison.OrdinalIgnoreCase));
    }
}
