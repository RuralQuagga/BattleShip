using BattleShip.Persistance.MongoDb.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleShip.Persistance.MongoDb.Configuration;

internal class BattleShipDbContext : DbContext
{
    public DbSet<GameSession> GameSessions { get; set; }

    public BattleShipDbContext(DbContextOptions<BattleShipDbContext> options) : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameSession>().ToCollection("gameSession");
        modelBuilder.Entity<GameField>().ToCollection("gameField");
        modelBuilder.Entity<GameHistory>()
            .ToCollection("gameHistory")
            .Property(e => e.ActionPoint)
            .HasConversion(
                point => JsonSerializer.Serialize(
                    new { X = point.X, Y = point.Y },
                    new JsonSerializerOptions()
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                        PropertyNamingPolicy = null
                    }),
                json => JsonSerializer.Deserialize<Point>(
                    json,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    })
            );
        modelBuilder.Entity<ShipEntity>()
            .ToCollection("ships")
            .Property(e => e.Points)
            .HasConversion(
            points => JsonSerializer.Serialize(
                points.Select(p => new { X = p.X, Y = p.Y }).ToArray(),
                new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                    PropertyNamingPolicy = null
                }),
            json => JsonSerializer.Deserialize<Point[]>(
                json,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                }) ?? Array.Empty<Point>()

        );
    }
}
