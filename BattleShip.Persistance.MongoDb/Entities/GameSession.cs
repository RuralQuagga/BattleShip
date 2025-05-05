using BattleShip.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BattleShip.Persistance.MongoDb.Entities;

public class GameSession
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public DateTime SessionStart { get; set; }

    public DateTime? SessionEnd { get; set; }

    [BsonRepresentation(BsonType.Int32)]
    public SessionState State { get; set; }
}
