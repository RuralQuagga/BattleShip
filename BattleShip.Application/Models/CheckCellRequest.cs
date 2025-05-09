namespace BattleShip.Application.Models;

public class CheckCellRequest
{
    public string FieldId { get; set; } = null!;

    public int Line { get; set; }

    public int Cell { get; set; }
}
