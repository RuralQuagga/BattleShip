namespace BattleShip.API.Contracts;

public class CheckCellApiRequest
{
    public string FieldId { get; set; } = null!;

    public int Line { get; set; }

    public int Cell { get; set; }
}
