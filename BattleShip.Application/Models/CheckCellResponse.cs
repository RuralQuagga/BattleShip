namespace BattleShip.Application.Models;

public class CheckCellResponse
{
    public GameFieldDto Field { get; set; } = null!;

    public bool IsSuccessCheck { get; set; }
}
