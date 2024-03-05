using Application.Abstractions.Data;
using Domain.Battles;
using Domain.Monsters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class BattleOfMonstersContext(DbContextOptions<BattleOfMonstersContext> dbContextOptions) : DbContext(dbContextOptions), IUnitOfWork
{
    public DbSet<Battle> Battles { get; set; } = null!;
    public DbSet<Monster> Monsters { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Monster>().HasMany<Battle>().WithOne(c => c.MonsterARelation).HasForeignKey(c => c.MonsterA).HasPrincipalKey(c => c.Id);
        modelBuilder.Entity<Monster>().HasMany<Battle>().WithOne(c => c.MonsterBRelation).HasForeignKey(c => c.MonsterB).HasPrincipalKey(c => c.Id);
        modelBuilder.Entity<Monster>().HasMany<Battle>().WithOne(c => c.WinnerRelation).HasForeignKey(c => c.MonsterWinnerId).HasPrincipalKey(c => c.Id);
    }
}
