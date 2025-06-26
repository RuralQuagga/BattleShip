using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BattleShip.Persistance.MongoDb.Entities;

public class GameHistory
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string RecordId { get; set; } = null!;

    public bool IsPlayerAction { get; set; }

    public Point ActionPoint { get; set; }

    public string SessionId { get; set; } = null!;

    public string FieldId { get; set; } = null!;

    public string? ShipId { get; set; }

    public bool IsSuccessAction { get; set; }

    public DateTime TimeStamp { get; set; }
}
