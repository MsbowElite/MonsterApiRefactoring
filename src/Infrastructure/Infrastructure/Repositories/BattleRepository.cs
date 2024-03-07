using Domain.Battles;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

internal sealed class BattleRepository(BattleOfMonstersContext context) : IBattleRepository
{
    public async ValueTask<Battle?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Battles.FindAsync(id, cancellationToken);
    }

    public async Task InsertAsync(Battle battle, CancellationToken cancellationToken)
    {
        await context.Battles.AddAsync(battle, cancellationToken);
    }

    public void Remove(Battle battle)
    {
        context.Battles.Remove(battle);
    }
}
