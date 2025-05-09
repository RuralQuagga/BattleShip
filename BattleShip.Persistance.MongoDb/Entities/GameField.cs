using BattleShip.Common.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BattleShip.Persistance.MongoDb.Entities;

public class GameField
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string FieldId { get; set; } = null!;

    public string SessionId { get; set; } = null!;

    public bool IsPlayerField { get; set; }
    
    public CellType[][] FieldConfiguration { get; set; } = Array.Empty<CellType[]>();
}
