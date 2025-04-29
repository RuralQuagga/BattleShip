using BattleShip.Common.Enums;

namespace BattleShip.Common.Helpers;

public static class EnumHelper
{
    public static int[][] ToIntMatrix(this CellType[][] field) =>
        field.Select(row => row.Select(c => (int)c).ToArray()).ToArray();
    
}
