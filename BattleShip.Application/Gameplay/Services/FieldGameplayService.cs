using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Helpers;
using BattleShip.Application.Mappers.Field;
using BattleShip.Application.Mappers.Session;
using BattleShip.Application.Models;
using BattleShip.Application.Objects.Field;
using BattleShip.Common.Enums;
using BattleShip.Persistance.MongoDb.Entities;
using BattleShip.Persistance.MongoDb.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using SharpCompress.Common;
using System.Drawing;

namespace BattleShip.Application.Gameplay.Services;

public class FieldGameplayService(
    IRepository<GameSession> sessionRepository,
    IRepository<GameField> fieldRepository,
    IShipService shipService,
    IHistoryService historyService) : IFieldGameplayService
{
    public async Task<GameFieldDto> GenerateBattleField(string sessionId, FieldType fieldType, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId, cancellationToken);

        if (session.State != SessionState.Preparing)
        {
            throw new InvalidOperationException($"Unable to generate field for session in {session.State} state");
        }
        var field = GenerateNewField();

        var fieldEntity = new GameField
        {
            FieldId = ObjectId.GenerateNewId().ToString(),
            IsPlayerField = fieldType == FieldType.User,
            SessionId = sessionId,
            FieldConfiguration = field.Field
        };

        await fieldRepository.AddAsync(fieldEntity, cancellationToken);
        LogField(fieldEntity, nameof(GenerateBattleField), "Generate field");

        await shipService.SaveShips(fieldEntity, cancellationToken);

        if (fieldType == FieldType.Computer)
        {
            fieldEntity = HideShip(fieldEntity);
        }

        LogField(fieldEntity, nameof(GenerateBattleField), "Hide generated field");

        return fieldEntity.ToDto();
    }

    private GameField HideShip(GameField fieldEntity)
    {
        for (var lineIndex = 0; lineIndex < fieldEntity.FieldConfiguration.Length; lineIndex++)
        {
            for (var cellIndex = 0; cellIndex < fieldEntity.FieldConfiguration.Length; cellIndex++)
            {
                if (fieldEntity.FieldConfiguration[lineIndex][cellIndex] == CellType.Ship || fieldEntity.FieldConfiguration[lineIndex][cellIndex] == CellType.Forbidden)
                {
                    fieldEntity.FieldConfiguration[lineIndex][cellIndex] = CellType.Empty;
                }
            }
        }

        return fieldEntity;
    }

    public async Task<SessionDto> ChangeSessionStateToInProgress(string sessionId, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        session.State = SessionState.InProgress;
        await sessionRepository.UpdateAsync(session, cancellationToken);

        return session.ToDto();
    }

    public async Task<SessionDto> StartNewSession(CancellationToken cancellationToken)
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
        var currentSession = await sessionRepository.GetByIdAsync(session.Id, cancellationToken);

        return currentSession.ToDto();
    }

    public async Task<GameFieldDto> RegenerateBattleField(string fieldId, CancellationToken cancellationToken)
    {
        var entity = await fieldRepository.GetByIdAsync(fieldId, cancellationToken);
        var newField = GenerateNewField();

        entity.FieldConfiguration = newField.Field;
        await fieldRepository.UpdateAsync(entity, cancellationToken);
        await shipService.DeleteOldShips(entity.FieldId, cancellationToken);
        await shipService.SaveShips(entity, cancellationToken);

        LogField(entity, nameof(RegenerateBattleField), "Regenerate field");
        return entity.ToDto();
    }

    public async Task<CheckCellResponse> CheckCell(CheckCellRequest request, CancellationToken cancellationToken)
    {
        var entity = await fieldRepository.GetByIdAsync(request.FieldId, cancellationToken);

        LogField(entity, nameof(CheckCell), "Field before check cell");
        var result = UpdateUnderMoveField(entity.FieldConfiguration, request.Line, request.Cell);
        entity.FieldConfiguration = result.Field;
        LogField(entity, nameof(CheckCell), "Field after check cell");
        await UpdateHistoryAndShip(request.FieldId, entity.SessionId, true, request.Line, request.Cell, cancellationToken);
        entity = await UpdateCellAroundDeadShip(entity, true, cancellationToken);
        LogField(entity, nameof(CheckCell), "Field after update forbidden cell and history");

        await fieldRepository.UpdateAsync(entity, cancellationToken);

        if (!entity.IsPlayerField)
        {
            entity = HideShip(entity);
        }

        var isAllShipsDead = await shipService.IsAllShipsDead(request.FieldId, cancellationToken);
        if (isAllShipsDead)
        {
            var session = await sessionRepository.GetByIdAsync(entity.SessionId, cancellationToken);
            session.State = SessionState.Win;
            session.SessionEnd = DateTime.Now;
            await sessionRepository.UpdateAsync(session, cancellationToken);
        }

        return new CheckCellResponse
        {
            Field = entity.ToDto(),
            IsSuccessCheck = isAllShipsDead ? ActionState.Win : result.ActionState
        };
    }

    public async Task<GameFieldDto> GetGameField(string sessionId, FieldType fieldType, CancellationToken cancellationToken)
    {
        var isPlayerField = fieldType == FieldType.User;
        var field = await fieldRepository.SingleOrDefault(f => f.SessionId.Equals(sessionId, StringComparison.OrdinalIgnoreCase)
                            && f.IsPlayerField == isPlayerField, cancellationToken);

        if (field is null)
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
            var pointToCheck = await GetNextPointToCheck(field, cancellationToken);

            LogField(field, nameof(GetComputerMove), "Field before check cell");
            result = UpdateUnderMoveField(field.FieldConfiguration, pointToCheck.Y, pointToCheck.X);
            field.FieldConfiguration = result.Field;
            LogField(field, nameof(GetComputerMove), "Field after check cell");
            await UpdateHistoryAndShip(field.FieldId, field.SessionId, false, pointToCheck.Y, pointToCheck.X, cancellationToken);
            field = await UpdateCellAroundDeadShip(field, false, cancellationToken);
            LogField(field, nameof(GetComputerMove), "Field after update forbidden cell and history");
            computerExecution = false;

        }

        await fieldRepository.UpdateAsync(field, cancellationToken);

        var isAllShipsDead = await shipService.IsAllShipsDead(fieldId, cancellationToken);
        if (isAllShipsDead)
        {
            var session = await sessionRepository.GetByIdAsync(field.SessionId, cancellationToken);
            session.State = SessionState.Loss;
            session.SessionEnd = DateTime.Now;
            await sessionRepository.UpdateAsync(session, cancellationToken);
        }

        return new CheckCellResponse
        {
            Field = field.ToDto(),
            IsSuccessCheck = isAllShipsDead ? ActionState.Lose : result.ActionState
        };
    }

    private async Task<Point> GetNextPointToCheck(GameField field, CancellationToken cancellationToken)
    {
        var history = await historyService.GetFieldHistory(field.FieldId, cancellationToken);
        var computerHistory = history.Where(h => h.IsPlayerAction == false);
        var isCheckSucceed = false;
        var pointToExecute = new Point();

        while (!isCheckSucceed)
        {
            var lastSuccessAction = computerHistory.LastOrDefault(a => a.IsSuccessAction);
            if (lastSuccessAction is null || lastSuccessAction.ShipId is null)
            {
                pointToExecute = GetRandomPoint(field);
                if (!computerHistory.Any(h => h.ActionPoint.Equals(pointToExecute)))
                {                    
                    return pointToExecute;
                }
                continue;
            }

            var lastActionedShip = await shipService.GetShipById(lastSuccessAction.ShipId, cancellationToken);

            if (lastActionedShip.State == ShipState.Dead)
            {
                pointToExecute = GetRandomPoint(field);
                if (!computerHistory.Any(h => h.ActionPoint.Equals(pointToExecute)))
                {
                    return pointToExecute;
                }
                continue;
            }

            if (IsFirstAim(computerHistory, lastActionedShip))
            {
                pointToExecute = GetNextPointFromNearCells(field, lastSuccessAction);
                if (!computerHistory.Any(h => h.ActionPoint.Equals(pointToExecute)))
                {
                    return pointToExecute;
                }
                continue;
            }

            pointToExecute = GetNextPointByDirection(computerHistory, lastActionedShip, field);

            if(!computerHistory.Any(h => h.ActionPoint.Equals(pointToExecute)))
            {
                isCheckSucceed = true;
            }
        }

        return pointToExecute;
    }

    private Point GetNextPointByDirection(IEnumerable<GameHistory> computerHistory, ShipEntity lastActionedShip, GameField field)
    {
        var aimedPoints = computerHistory.Where(h => h.IsSuccessAction
        && h.ShipId is not null
        && h.ShipId.Equals(lastActionedShip.ShipId, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.TimeStamp)
            .Select(a => a.ActionPoint)
            .ToList();

        var nexPoint = PointSequenceAnalyzer.GetNextOrPreviousPoint(aimedPoints, field);
        return nexPoint ?? GetRandomPoint(field);
    }

    private Point GetRandomPoint(GameField field)
    {
        var random = new Random(DateTime.Now.Microsecond);        
        var freeCell = field.FieldConfiguration.SelectMany((row, rowIndex) =>
        row.Select((cell, colIndex) => (cell, rowIndex, colIndex))
    .Where(x => x.cell == CellType.Ship || x.cell == CellType.Empty || x.cell == CellType.Forbidden)
    .Select(x => new Point(x.colIndex, x.rowIndex))
    .ToList());

        return freeCell.ElementAt(random.Next(freeCell.Count()));
    }

    private Point GetNextPointFromNearCells(GameField field, GameHistory lastSuccessAction)
    {
        var pointsSelectFrom = new List<Point>
        {
            new Point(lastSuccessAction.ActionPoint.X, lastSuccessAction.ActionPoint.Y - 1),
            new Point(lastSuccessAction.ActionPoint.X, lastSuccessAction.ActionPoint.Y + 1),
            new Point(lastSuccessAction.ActionPoint.X - 1, lastSuccessAction.ActionPoint.Y),
            new Point(lastSuccessAction.ActionPoint.X + 1, lastSuccessAction.ActionPoint.Y)
        };

        pointsSelectFrom = FilterAlreadyMissedPoints(field, pointsSelectFrom);

        var random = new Random(DateTime.Now.Microsecond);

        return pointsSelectFrom[random.Next(pointsSelectFrom.Count)];
    }

    private List<Point> FilterAlreadyMissedPoints(GameField field, List<Point> potentialPoints)
    {
        var pointsToReturn = new List<Point>();
        foreach (var point in potentialPoints)
        {
            if(point.X < 0 || point.X >= field.FieldConfiguration.Length
                || point.Y < 0 || point.Y >= field.FieldConfiguration.Length)
            {
                continue;
            }

            if (field.FieldConfiguration[point.Y][point.X] != CellType.Miss
                || field.FieldConfiguration[point.Y][point.X] != CellType.ForbiddenMiss
                || field.FieldConfiguration[point.Y][point.X] != CellType.DeadShip)
            {
                pointsToReturn.Add(point);
            }
        }

        return pointsToReturn;
    }

    private bool IsFirstAim(IEnumerable<GameHistory> computerHistory, ShipEntity lastActionedShip) =>
        computerHistory.Count(h => h.IsSuccessAction
        && h.ShipId is not null
        && h.ShipId.Equals(lastActionedShip.ShipId, StringComparison.OrdinalIgnoreCase)) == 1;

    public async Task<string?> CheckSessionInProgress(CancellationToken cancellationToken)
    {
        var session = await sessionRepository.SingleOrDefault(s => s.State == SessionState.InProgress, cancellationToken);

        return session?.Id;
    }

    private UpdateUnderMoveFieldDto UpdateUnderMoveField(CellType[][] field, int line, int cell)
    {
        var isSucceedCheck = ActionState.Fail;

        switch (field[line][cell])
        {
            case CellType.Ship:
                field[line][cell] = CellType.DeadShip;
                isSucceedCheck = ActionState.Success;
                break;
            case CellType.Forbidden:
                field[line][cell] = CellType.ForbiddenMiss;
                break;
            case CellType.DeadShip:
                break;
            default:
                field[line][cell] = CellType.Miss;
                break;
        }

        return new UpdateUnderMoveFieldDto
        {
            Field = field,
            ActionState = isSucceedCheck
        };
    }

    private BattleField GenerateNewField()
    {
        var field = new BattleField(10);

        field.PrepareField();
        field.GenerateField();

        return field;
    }

    private async Task CloseOngoingSessions(IEnumerable<GameSession> sessions, CancellationToken cancellationToken)
    {
        foreach (var session in sessions)
        {
            session.State = SessionState.Closed;
            session.SessionEnd = DateTime.Now;

            await sessionRepository.UpdateAsync(session, cancellationToken);
        }
    }

    private async Task UpdateHistoryAndShip(string fieldId, string sessionId, bool isPlayerAction, int lineIndex, int cellIndex, CancellationToken cancellationToken)
    {
        var shipUnderAttack = await shipService.GetShipByPlace(fieldId, lineIndex, cellIndex, cancellationToken);

        await historyService.AddRecordToHistory(fieldId, sessionId, shipUnderAttack?.ShipId, isPlayerAction, lineIndex, cellIndex, cancellationToken);

        if (shipUnderAttack is not null)
        {
            var isDeadShip = await historyService.CheckIsShipDead(shipUnderAttack, cancellationToken);
            await shipService.UpdateShip(shipUnderAttack.ShipId, isDeadShip, cancellationToken);
        }
    }

    private async Task<GameField> UpdateCellAroundDeadShip(GameField field, bool isPlayerAction, CancellationToken cancellationToken)
    {
        var fieldHistory = await historyService.GetFieldHistory(field.FieldId, cancellationToken);
        var shipsFromHistory = fieldHistory.Where(h => h.IsPlayerAction == isPlayerAction && h.ShipId is not null)
            .GroupBy(h => h.ShipId).DistinctBy(h => h.Key);

        foreach (var ship in shipsFromHistory)
        {
            if (ship is null || ship.Key is null)
            {
                throw new InvalidOperationException("Uable to get ship from history");
            }

            var shipEntity = await shipService.GetShipById(ship.Key, cancellationToken);

            if (shipEntity.State == ShipState.Dead)
            {
                field = UpdateFieldAroundShip(field, shipEntity, cancellationToken);
            }
        }

        return field;
    }

    private GameField UpdateFieldAroundShip(GameField field, ShipEntity shipEntity, CancellationToken cancellationToken)
    {
        foreach (var point in shipEntity.Points)
        {
            var pointsToUpdate = new List<Point>
            {
                new Point(point.X - 1, point.Y),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X, point.Y + 1),
                new Point(point.X - 1, point.Y - 1),
                new Point(point.X - 1, point.Y + 1),
                new Point(point.X + 1, point.Y - 1),
                new Point(point.X + 1, point.Y + 1),
            };

            foreach (var pointToCheck in pointsToUpdate)
            {
                if (pointToCheck.X < 0
                    || pointToCheck.X >= field.FieldConfiguration.Length
                    || pointToCheck.Y < 0
                    || pointToCheck.Y >= field.FieldConfiguration.Length
                    || shipEntity.Points.Contains(pointToCheck))
                {
                    continue;
                }

                if (field.FieldConfiguration[pointToCheck.Y][pointToCheck.X] != CellType.Ship
                    || field.FieldConfiguration[pointToCheck.Y][pointToCheck.X] != CellType.DeadShip)
                {
                    field.FieldConfiguration[pointToCheck.Y][pointToCheck.X] = CellType.ForbiddenMiss;
                }
            }
        }

        return field;
    }

    private void LogField(GameField field, string methodName, string info = "")
    {
        Console.WriteLine($"--------------{methodName}:{DateTime.Now}--------------");
        foreach (var line in field.FieldConfiguration)
        {
            foreach (var cell in line)
            {
                Console.Write(GetCell(cell));
            }
            Console.WriteLine();
        }
        Console.WriteLine(info);
    }

    private string GetCell(CellType cell) =>
        cell switch
        {
            CellType.Ship => "S",
            CellType.Empty => "e",
            CellType.Forbidden => "f",
            CellType.DeadShip => "d",
            CellType.Miss => "M",
            CellType.ForbiddenMiss => "m",
            _ => throw new InvalidOperationException()
        };

    public async Task<StatisticModel> GetSessionStatistic(string sessionId, CancellationToken cancellationToken)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId, cancellationToken);
        var history = await historyService.GetSessionHistory(sessionId, cancellationToken);

        if(session is null || history is null || history.Count() == 0)
        {
            return new StatisticModel();
        }

        return new StatisticModel
        {
            GameTimeMs = (session.SessionEnd - session.SessionStart)?.Microseconds ?? 0,
            YourMoves = history.Count(h => h.IsPlayerAction),
            EnemyMoves = history.Count(h => !h.IsPlayerAction),
            HitPercentage = (float)history.Count(h => h.IsPlayerAction && h.IsSuccessAction) / history.Count(h => h.IsPlayerAction) * 100
        };
    }

    public async Task<List<StatisticModel>> GetFullStatistic(CancellationToken cancellationToken)
    {
        var sessions = await sessionRepository.GetAllAsync(cancellationToken);
        var sessionsToCheck = sessions.Where(s => s.State == SessionState.Win || s.State == SessionState.Loss);
        var result = new List<StatisticModel>();

        foreach(var session in sessionsToCheck)
        {
            var statistic = await GetSessionStatistic(session.Id, cancellationToken);
            result.Add(statistic);
        }

        return result;
    }

    public async Task ClearStatistic(CancellationToken cancellationToken)
    {
        await sessionRepository.DeleteAll(cancellationToken);
        await fieldRepository.DeleteAll(cancellationToken);
        await shipService.DeleteAll(cancellationToken);
        await historyService.DeleteAll(cancellationToken);
    }
}
