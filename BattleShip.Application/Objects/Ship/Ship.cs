using BattleShip.Common.Enums;

namespace BattleShip.Application.Objects.Ship;

internal class Ship
{
    public int Size { get; set; }

    public Orientation Orientation { get; set; }

    public int LineIndex { get; set; }

    public int[] CellsIndexes { get; set; } = Array.Empty<int>();


}
