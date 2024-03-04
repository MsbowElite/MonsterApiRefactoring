using Application.Abstractions.Data;
using Domain.Monsters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class BattleOfMonstersReadDbContext(DbContextOptions<BattleOfMonstersReadDbContext> options)
    : BattleOfMonstersContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BattleOfMonstersReadDbContext).Assembly,
            ReadConfigurationsFilter);
    }

    private static bool ReadConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Configurations.Read") ?? false;
}
