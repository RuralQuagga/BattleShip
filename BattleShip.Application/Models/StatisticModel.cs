namespace BattleShip.Application.Models;

public class StatisticModel
{
    public int GameTimeMs { get; set; }

    public int YourMoves { get; set; }

    public int EnemyMoves { get; set; }

    public float HitPercentage { get; set; }
}
