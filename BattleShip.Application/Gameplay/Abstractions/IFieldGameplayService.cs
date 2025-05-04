using BattleShip.Common.Enums;

namespace BattleShip.Application.Gameplay.Abstractions;

public interface IFieldGameplayService
{
    Task<CellType[][]> GenerateBattleField();
}
