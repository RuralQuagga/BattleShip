using BattleShip.Application.Gameplay.Abstractions;
using BattleShip.Application.Objects.Field;
using BattleShip.Common.Enums;
using Moq;

namespace BattleShip.Application.Tests.Battlefield
{
    public class GenerateFieldTests
    {
        [Fact]
        public void RotateGameField()
        {
            var field = new BattleField(4);
            field.PrepareField();
            for(var i = 0; i < 4; i++)
            {
                field.Field[i][2] = CellType.Forbidden;
            }

            field.RotateShip();

            var rotatedRow = field.Field[2];
            var expectedResult = new CellType[] { CellType.Forbidden, CellType.Forbidden, CellType.Forbidden, CellType.Forbidden };

            Assert.Equal(expectedResult, rotatedRow);
        }        
    }
}
