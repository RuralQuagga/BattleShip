using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Common.Enums;
using BattleShip.Persistance.MongoDb.Entities;
using BattleShip.Persistance.MongoDb.Repository;
using MongoDB.Bson;
using System.Drawing;

namespace BattleShip.Application.Gameplay.Services;

internal class ShipService(IRepository<ShipEntity> shipRepository) : IShipService
{  
    public async Task UpdateShip(string shipId, bool isDeadShip, CancellationToken cancellationToken)
    {
        var result = await shipRepository.GetByIdAsync(shipId, cancellationToken);

        if (isDeadShip)
        {
            result.State = Common.Enums.ShipState.Dead;
            await shipRepository.UpdateAsync(result, cancellationToken);
            return;
        }

        if (result.State == Common.Enums.ShipState.UnderAttack)
            return;

        result.State = Common.Enums.ShipState.UnderAttack;
        await shipRepository.UpdateAsync(result, cancellationToken);
    }

    public async Task<ShipEntity?> GetShipByPlace(string fieldId, int lineIndex, int cellIndex, CancellationToken cancellationToken)
    {
        var ships = await shipRepository.GetAllAsync(cancellationToken);

        return ships.SingleOrDefault(s => s.FieldId.Equals(fieldId, StringComparison.OrdinalIgnoreCase)
        && s.Points.Any(p => p.Equals(new Point(cellIndex, lineIndex))));
    }

    public async Task<ShipEntity> GetShipById(string shipId, CancellationToken cancellationToken) =>
        await shipRepository.GetByIdAsync(shipId, cancellationToken);    

    public async Task SaveShips(GameField field, CancellationToken cancellationToken)
    {
        var points = field.FieldConfiguration.SelectMany((row, rowIndex) =>
        row.Select((cell, colIndex) => (cell, rowIndex, colIndex))
    .Where(x => x.cell == CellType.Ship)
    .Select(x => new Point(x.colIndex, x.rowIndex))
    .ToList());

        var groupedPoints = GroupPoints(points.ToList());

        foreach(var group in groupedPoints)
        {
            var ship = new ShipEntity
            {
                ShipId = ObjectId.GenerateNewId().ToString(),
                FieldId = field.FieldId,
                Points = group.ToArray(),
                Size = group.Count(),
                State = ShipState.Alive
            };

            await shipRepository.AddAsync(ship, cancellationToken);
        }
    }

    public static List<List<Point>> GroupPoints(List<Point> points)
    {
        var result = new List<List<Point>>();
        var visited = new HashSet<Point>();
        var pointSet = new HashSet<Point>(points);

        foreach (var point in points)
        {
            if (!visited.Contains(point))
            {
                var group = new List<Point>();
                ExploreGroup(point, pointSet, visited, group);

                if (group.Count > 0)
                {
                    group = SortGroup(group);
                    result.Add(group);
                }
            }
        }

        return result;
    }

    private static void ExploreGroup(Point point, HashSet<Point> pointSet,
                                   HashSet<Point> visited, List<Point> group)
    {
        if (!pointSet.Contains(point)) return;
        if (visited.Contains(point)) return;

        visited.Add(point);
        group.Add(point);

        var directions = new Point[]
        {
            new Point(1, 0),  
            new Point(-1, 0), 
            new Point(0, 1),   
            new Point(0, -1)  
        };

        foreach (var dir in directions)
        {
            var neighbor = new Point(point.X + dir.X, point.Y + dir.Y);
            ExploreGroup(neighbor, pointSet, visited, group);
        }
    }

    private static List<Point> SortGroup(List<Point> group)
    {
        if (group.Select(p => p.Y).Distinct().Count() == 1)
        {
            return group.OrderBy(p => p.X).ToList();
        }
       
        else if (group.Select(p => p.X).Distinct().Count() == 1)
        {
            return group.OrderBy(p => p.Y).ToList();
        }

        return group.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
    }

    public async Task DeleteOldShips(string fieldId, CancellationToken cancellationToken)
    {
        var ships = await shipRepository.GetAllAsync(cancellationToken);

        var shipIdsToDelete = ships.Where(s => s.FieldId.Equals(fieldId, StringComparison.OrdinalIgnoreCase)).Select(s => s.ShipId);

        foreach(var shipId in shipIdsToDelete)
        {
            await shipRepository.DeleteAsync(shipId, cancellationToken);
        }
    }

    public async Task<bool> IsAllShipsDead(string fieldId, CancellationToken cancellationToken)
    {
        var ships = await shipRepository.GetAllAsync(cancellationToken);

        return ships.Where(s => s.FieldId.Equals(fieldId, StringComparison.OrdinalIgnoreCase)).All(s => s.State == ShipState.Dead);
    }
}
