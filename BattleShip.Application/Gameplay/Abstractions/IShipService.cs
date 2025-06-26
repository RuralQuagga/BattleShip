using BattleShip.Persistance.MongoDb.Entities;

namespace BattleShip.Application.Gameplay.Abstractions;

public interface IShipService
{
    Task UpdateShip(string shipId, bool isDeadShip, CancellationToken cancellationToken);
   
    Task<ShipEntity?> GetShipByPlace(string fieldId, int lineIndex, int cellIndex, CancellationToken cancellationToken);

    Task<ShipEntity> GetShipById(string shipId, CancellationToken cancellationToken);

    Task SaveShips(GameField field, CancellationToken cancellationToken);

    Task DeleteOldShips(string fieldId, CancellationToken cancellationToken);

    Task<bool> IsAllShipsDead(string fieldId, CancellationToken cancellationToken);
}
