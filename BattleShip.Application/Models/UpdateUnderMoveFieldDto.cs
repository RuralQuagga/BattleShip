using BattleShip.Common.Enums;

namespace BattleShip.Application.Models;

internal class UpdateUnderMoveFieldDto
{
    internal CellType[][] Field { get; set; } = null!;

    internal ActionState ActionState { get; set; }
}
