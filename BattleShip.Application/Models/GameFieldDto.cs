using BattleShip.Common.Enums;

namespace BattleShip.Application.Models;

public class GameFieldDto
{
    public string FieldId { get; set; } = null!;

    public string SessionId { get; set; } = null!;

    public bool IsPlayerField { get; set; }
    
    public int[][] FieldConfiguration { get; set; } = Array.Empty<int[]>();
}
