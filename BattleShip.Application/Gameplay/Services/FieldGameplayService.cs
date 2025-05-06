using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Objects.Field;
using BattleShip.Common.Enums;
using BattleShip.Persistance.MongoDb.Entities;
using BattleShip.Persistance.MongoDb.Repository;
using MongoDB.Bson;

namespace BattleShip.Application.Gameplay.Services;

internal class FieldGameplayService(IRepository<GameSession> sessionRepository) : IFieldGameplayService
{
    public async Task<CellType[][]> GenerateBattleField()
    {
        var field = new BattleField(10);

        field.PrepareField();
        field.GenerateField();

        return await Task.FromResult(field.Field);
    }

    public async Task<string> ChangeSessionStateToInProgress(string sessionId, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId, cancellationToken);       

        if(session.State != SessionState.Preparing)
        {
            throw new InvalidOperationException($"State of session with Id {session.Id} is {session.State}");
        }

        session.State = SessionState.InProgress;
        await sessionRepository.UpdateAsync(session, cancellationToken);

        return session.Id;
    }

    public async Task<string> StartNewSession(CancellationToken cancellationToken)
    {
        var sessions = await sessionRepository.GetAllAsync(cancellationToken);
        var ongoingSessions = sessions.Where(s => s.State == SessionState.Preparing || s.State == SessionState.InProgress);

        if (ongoingSessions.Any())
        {
            await CloseOngoingSessions(ongoingSessions, cancellationToken);
        }

        var session = new GameSession
        {
            Id = ObjectId.GenerateNewId().ToString(),
            SessionStart = DateTime.Now,
            State = SessionState.Preparing
        };

        await sessionRepository.AddAsync(session, cancellationToken);

        return session.Id;
    }

    private async Task CloseOngoingSessions(IEnumerable<GameSession> sessions, CancellationToken cancellationToken)
    {
        foreach(var session in sessions)
        {
            session.State = SessionState.Closed;
            session.SessionEnd = DateTime.Now;

            await sessionRepository.UpdateAsync(session, cancellationToken);
        }
    }
}
