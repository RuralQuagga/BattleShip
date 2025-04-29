using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Objects.Field;
using BattleShip.Common.Enums;

namespace BattleShip.Application.Gameplay.Services;

internal class FieldGameplayService : IFieldGameplayService
{
    public async Task<CellType[][]> GenerateBattleField()
    {
        var field = new BattleField(10);

        field.PrepareField();
        field.GenerateField();

        return await Task.FromResult(field.Field);
    }
}
