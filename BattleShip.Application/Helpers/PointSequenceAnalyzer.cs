using BattleShip.Common.Enums;
using BattleShip.Persistance.MongoDb.Entities;
using System.Drawing;

namespace BattleShip.Application.Helpers;

public class PointSequenceAnalyzer
{
    public static Point? GetNextOrPreviousPoint(List<Point> points, GameField field)
    {
        if (points == null || points.Count < 2)
            return null;

        var direction = GetSequenceDirection(points);
        if (direction == null)
            return null;
        
        var ordered = GetOrderedPoints(points, direction.Value);
        var first = ordered.First();
        var last = ordered.Last();
        
        var next = new Point(
            last.X + direction.Value.X,
            last.Y + direction.Value.Y
        );
      
        if (IsWithinBounds(next, field))
        {
            return next;
        }
        else
        {            
            var previous = new Point(
                first.X - direction.Value.X,
                first.Y - direction.Value.Y
            );

            return IsWithinBounds(previous, field) ? previous : (Point?)null;
        }
    }

    private static bool IsWithinBounds(Point p, GameField field)
    {
        return p.X >= 0 && p.X < field.FieldConfiguration.Length &&
               p.Y >= 0 && p.Y < field.FieldConfiguration.Length &&
               field.FieldConfiguration[p.Y][p.X] != CellType.ForbiddenMiss &&
               field.FieldConfiguration[p.Y][p.X] != CellType.Miss &&
               field.FieldConfiguration[p.Y][p.X] != CellType.DeadShip;
    }

    private static List<Point> GetOrderedPoints(List<Point> points, Point direction)
    {        
        if (direction.Y == 0)
            return points.OrderBy(p => p.X).ToList();
        
        return points.OrderBy(p => p.Y).ToList();
    }

    private static Point? GetSequenceDirection(List<Point> points)
    {        
        if (points.All(p => p.Y == points[0].Y))
        {
            var ordered = points.OrderBy(p => p.X).ToList();
            int step = ordered[1].X - ordered[0].X;
            return new Point(step, 0);
        }
        
        if (points.All(p => p.X == points[0].X))
        {
            var ordered = points.OrderBy(p => p.Y).ToList();
            int step = ordered[1].Y - ordered[0].Y;
            return new Point(0, step);
        }

        return null;
    }
}
