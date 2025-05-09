using BattleShip.Application.Models;
using BattleShip.Common.Helpers;
using BattleShip.Persistance.MongoDb.Entities;

namespace BattleShip.Application.Mappers.Field;

public static class EntityFieldToDtoMapper
{
    public static GameFieldDto ToDto(this GameField field)
    {
        return new GameFieldDto
        {
            FieldId = field.FieldId,
            SessionId = field.SessionId,
            IsPlayerField = field.IsPlayerField,
            FieldConfiguration = field.FieldConfiguration.ToIntMatrix()
        };
    }
}
