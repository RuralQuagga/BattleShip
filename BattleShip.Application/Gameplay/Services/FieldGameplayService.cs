using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Mappers.Field;
using BattleShip.Application.Models;
using BattleShip.Application.Objects.Field;
using BattleShip.Common.Enums;
using BattleShip.Persistance.MongoDb.Entities;
using BattleShip.Persistance.MongoDb.Repository;
using MongoDB.Bson;

namespace BattleShip.Application.Gameplay.Services;

internal class FieldGameplayService(
    IRepository<GameSession> sessionRepository,
    IRepository<GameField> fieldRepository) : IFieldGameplayService
{
    public async Task<GameFieldDto> GenerateBattleField(string sessionId, FieldType fieldType, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId, cancellationToken);

        if(session.State != SessionState.Preparing)
        {
            throw new InvalidOperationException($"Unable to generate field for session in {session.State} state");
        }

        var field = GenerateNewField();

        var fieldEntity = new GameField
        {
            FieldId = ObjectId.GenerateNewId().ToString(),
            IsPlayerField = fieldType == FieldType.User,
            SessionId = sessionId,
            FieldConfiguration = field
        };

        await fieldRepository.AddAsync(fieldEntity, cancellationToken);
        
        if(fieldType == FieldType.Computer)
        {
            fieldEntity = HideShip(fieldEntity);
        }

        return fieldEntity.ToDto();
    }

    private GameField HideShip(GameField fieldEntity)
    {
        for(var lineIndex = 0; lineIndex < fieldEntity.FieldConfiguration.Length; lineIndex ++)
        {
            for(var cellIndex = 0; cellIndex < fieldEntity.FieldConfiguration.Length; cellIndex++)
            {
                if (fieldEntity.FieldConfiguration[lineIndex][cellIndex] == CellType.Ship || fieldEntity.FieldConfiguration[lineIndex][cellIndex] == CellType.Forbidden)
                {
                    fieldEntity.FieldConfiguration[lineIndex][cellIndex] = CellType.Empty;
                }
            }
        }

        return fieldEntity;
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

    public async Task<GameFieldDto> RegenerateBattleField(string fieldId, CancellationToken cancellationToken)
    {
        var entity = await fieldRepository.GetByIdAsync(fieldId, cancellationToken);
        var newField = GenerateNewField();

        entity.FieldConfiguration = newField;
        await fieldRepository.UpdateAsync(entity, cancellationToken);

        return entity.ToDto();
    }

    public async Task<GameFieldDto> CheckCell(CheckCellRequest request, CancellationToken cancellationToken)
    {
        var entity = await fieldRepository.GetByIdAsync(request.FieldId, cancellationToken);

        switch (entity.FieldConfiguration[request.Line][request.Cell])
        {
            case CellType.Ship:
                entity.FieldConfiguration[request.Line][request.Cell] = CellType.DeadShip;
                break;
            case CellType.Forbidden:
                entity.FieldConfiguration[request.Line][request.Cell] = CellType.ForbiddenMiss;
                break;
            default:
                entity.FieldConfiguration[request.Line][request.Cell] = CellType.Miss;
                break;
        }

        await fieldRepository.UpdateAsync(entity, cancellationToken);

        if (!entity.IsPlayerField)
        {
            entity = HideShip(entity);
        }

        return entity.ToDto();
    }

    private CellType[][] GenerateNewField()
    {
        var field = new BattleField(10);

        field.PrepareField();
        field.GenerateField();

        return field.Field;
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
