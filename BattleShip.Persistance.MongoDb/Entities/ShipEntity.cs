using BattleShip.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BattleShip.Persistance.MongoDb.Entities;

public class ShipEntity
{
    [Key]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ShipId { get; set; } = null!;

    public Point[] Points { get; set; }   

    public ShipState State { get; set; }

    public string FieldId { get; set; } = null!;

    public int Size { get; set; }
}
