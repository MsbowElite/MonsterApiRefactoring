using Application.Abstractions.Data;
using Domain.Monsters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class BattleOfMonstersWriteDbContext(DbContextOptions<BattleOfMonstersWriteDbContext> options)
    : BattleOfMonstersContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BattleOfMonstersWriteDbContext).Assembly,
            WriteConfigurationsFilter);
    }

    private static bool WriteConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Configurations.Write") ?? false;
}
