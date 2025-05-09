using BattleShip.API.Contracts;
using BattleShip.Application.Models;

namespace BattleShip.API.Mappers.Field
{
    public static class ApiModelToDto
    {
        public static CheckCellRequest ToDto(this CheckCellApiRequest request)
        {
            return new CheckCellRequest
            {
                FieldId = request.FieldId,
                Line = request.Line,
                Cell = request.Cell
            };
        }
    }
}
