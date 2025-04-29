using BattleShip.Application.Objects.Field;
using BattleShip.Common.Enums;

namespace BattleShip.Application.Tests.Battlefield;

public class PrepareFieldTest
{
    [Fact]
    public void PrepareField_ShouldReturnFieldWithAllEmptyCell()
    {
        var field = new BattleField(10);

        field.PrepareField();

        Assert.True(field.Field.Length != 0);
        Assert.True(field.Field[2][2] == CellType.Empty);
    }
}
