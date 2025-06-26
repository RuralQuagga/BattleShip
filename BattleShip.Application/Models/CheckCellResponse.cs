using BattleShip.Common.Enums;

namespace BattleShip.Application.Models;

public class CheckCellResponse
{
    public GameFieldDto Field { get; set; } = null!;

    public ActionState IsSuccessCheck { get; set; }
}
