using Lib.Repository.Entities;
using Lib.Repository.Mappings;
using Lib.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace Lib.Repository;

public sealed class BattleOfMonstersContext : DbContext, IUnitOfWork
{
    public DbSet<Battle> Battle { get; set; } = null!;
    public DbSet<Monster> Monster { get; set; } = null!;


    public BattleOfMonstersContext(DbContextOptions<BattleOfMonstersContext> options) : base(options) { }

    public async Task<bool> Commit()
    {
        return await base.SaveChangesAsync(true) > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfiguration(new BattleMapping());
        modelBuilder.ApplyConfiguration(new MonsterMapping());

        modelBuilder.Entity<Monster>().HasMany<Battle>().WithOne(c => c.MonsterARelation).HasForeignKey(c => c.MonsterA).HasPrincipalKey(c => c.Id);
        modelBuilder.Entity<Monster>().HasMany<Battle>().WithOne(c => c.MonsterBRelation).HasForeignKey(c => c.MonsterB).HasPrincipalKey(c => c.Id);
        modelBuilder.Entity<Monster>().HasMany<Battle>().WithOne(c => c.WinnerRelation).HasForeignKey(c => c.Winner).HasPrincipalKey(c => c.Id);
    }
}