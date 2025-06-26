using BattleShip.Application.Models;
using BattleShip.Persistance.MongoDb.Entities;

namespace BattleShip.Application.Mappers.Session;

public static class SessionEntityToDtoMapper
{
    public static SessionDto ToDto(this GameSession entity) =>
        new SessionDto
        {
            Id = entity.Id,
            SessionEnd = entity.SessionEnd,
            SessionStart = entity.SessionStart,
            State = entity.State
        };
}
