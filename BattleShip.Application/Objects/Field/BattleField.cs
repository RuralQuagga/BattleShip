using BattleShip.Common.Enums;

namespace BattleShip.Application.Objects.Field;

internal class BattleField
{
    public int Size { get; set; }

    public List<List<CellType>> Field { get; set; } = null!;

    public bool FillingIsCompleted { get; set; }

    public BattleField(int size)
    {
        Size = size;        
    }

    public void PrepareField()
    {
        Field = new List<List<CellType>>(Size);

        for(var i = 0; i < Size; i++)
        {
            Field[i] = new List<CellType>(Size);
            
            for(var j = 0; j < Size; j++)
            {
                Field[i][j] = CellType.Empty;
            }
        }
    }

    public void GenerateField()
    {

    }
}
