using BattleShip.Application.Models;
using BattleShip.Common.Enums;

namespace BattleShip.Application.Gameplay.Abstractions;

public interface IFieldGameplayService
{
    Task<GameFieldDto> GenerateBattleField(string sessionId, FieldType fieldType, CancellationToken cancellationToken);

    Task<GameFieldDto> RegenerateBattleField(string fieldId, CancellationToken cancellationToken);

    Task<SessionDto> StartNewSession(CancellationToken cancellationToken);

    Task<SessionDto> ChangeSessionStateToInProgress(string sessionId, CancellationToken cancellationToken);

    Task<CheckCellResponse> CheckCell(CheckCellRequest request, CancellationToken cancellationToken);

    Task<GameFieldDto> GetGameField(string sessionId, FieldType fieldType, CancellationToken cancellationToken);

    Task<CheckCellResponse> GetComputerMove(string fieldId, CancellationToken cancellationToken);

    Task<string?> CheckSessionInProgress(CancellationToken cancellationToken);
}
