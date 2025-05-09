using BattleShip.Persistance.MongoDb.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace BattleShip.Persistance.MongoDb.Configuration;

internal class BattleShipDbContext: DbContext
{
    public DbSet<GameSession> GameSessions { get; set; }

    public BattleShipDbContext(DbContextOptions<BattleShipDbContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameSession>().ToCollection("gameSession");
        modelBuilder.Entity<GameField>().ToCollection("gameField");
    }
}
