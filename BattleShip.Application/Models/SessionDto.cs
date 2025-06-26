using BattleShip.Common.Enums;

namespace BattleShip.Application.Models;

public class SessionDto
{
    public string Id { get; set; } = null!;

    public DateTime SessionStart { get; set; }

    public DateTime? SessionEnd { get; set; }
    
    public SessionState State { get; set; }
}
