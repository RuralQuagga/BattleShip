using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Mappers.Field;
using BattleShip.Application.Models;
using BattleShip.Application.Objects.Field;
using BattleShip.Common.Enums;
using BattleShip.Persistance.MongoDb.Entities;
using BattleShip.Persistance.MongoDb.Repository;
using MongoDB.Bson;
using SharpCompress.Common;

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

    public async Task<CheckCellResponse> CheckCell(CheckCellRequest request, CancellationToken cancellationToken)
    {
        var entity = await fieldRepository.GetByIdAsync(request.FieldId, cancellationToken);        

        var result = UpdateUnderMoveField(entity.FieldConfiguration, request.Line, request.Cell);
        entity.FieldConfiguration = result.Field;

        await fieldRepository.UpdateAsync(entity, cancellationToken);

        if (!entity.IsPlayerField)
        {
            entity = HideShip(entity);
        }

        return new CheckCellResponse
        {
            Field = entity.ToDto(),
            IsSuccessCheck = result.IsSuccessCheck
        };
    }

    public async Task<GameFieldDto> GetGameField(string sessionId, FieldType fieldType, CancellationToken cancellationToken)
    {
        var isPlayerField = fieldType == FieldType.User;
        var field = await fieldRepository.SingleOrDefault(f => f.SessionId.Equals(sessionId, StringComparison.OrdinalIgnoreCase) 
                            && f.IsPlayerField == isPlayerField, cancellationToken);

        if(field is null)
        {
            var fieldName = isPlayerField ? "User" : "Computer";
            throw new InvalidOperationException($"Session with id {sessionId} do not have {fieldName} field");
        }

        if (!field.IsPlayerField)
        {
            field = HideShip(field);
        }

        return field.ToDto();
    }

    public async Task<CheckCellResponse> GetComputerMove(string fieldId, CancellationToken cancellationToken)
    {
        var field = await fieldRepository.GetByIdAsync(fieldId, cancellationToken);

        var random = new Random(DateTime.Now.Microsecond);
        var computerExecution = true;
        var result = new UpdateUnderMoveFieldDto();

        while (computerExecution)
        {
            var lineIndex = random.Next(field.FieldConfiguration.Length);
            var cellIndex = random.Next(field.FieldConfiguration.Length);

            if (field.FieldConfiguration[lineIndex][cellIndex] != CellType.Miss 
                || field.FieldConfiguration[lineIndex][cellIndex] != CellType.ForbiddenMiss)
            {
                result = UpdateUnderMoveField(field.FieldConfiguration, lineIndex, cellIndex);
                field.FieldConfiguration = result.Field;
                computerExecution = false;
            }
        }

        await fieldRepository.UpdateAsync(field, cancellationToken);
        return new CheckCellResponse
        {
            Field = field.ToDto(),
            IsSuccessCheck = result.IsSuccessCheck
        };
    }

    public async Task<string?> CheckSessionInProgress(CancellationToken cancellationToken)
    {
        var session = await sessionRepository.SingleOrDefault(s => s.State == SessionState.InProgress, cancellationToken);

        return session?.Id;
    }

    private UpdateUnderMoveFieldDto UpdateUnderMoveField(CellType[][] field, int line, int cell)
    {
        var isSucceedCheck = false;

        switch (field[line][cell])
        {
            case CellType.Ship:
                field[line][cell] = CellType.DeadShip;
                isSucceedCheck = true;
                break;
            case CellType.Forbidden:
                field[line][cell] = CellType.ForbiddenMiss;
                break;
            default:
                field[line][cell] = CellType.Miss;
                break;
        }

        return new UpdateUnderMoveFieldDto
        {
            Field = field,
            IsSuccessCheck = isSucceedCheck
        };
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
