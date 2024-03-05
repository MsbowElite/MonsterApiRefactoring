using Domain.Monsters;
using Infrastructure.Database;

namespace Infrastructure.Repositories;

internal sealed class MonsterRepository(BattleOfMonstersContext context) : IMonsterRepository
{
    public async Task InsertAsync(Monster monster, CancellationToken cancellationToken)
    {
        await context.Monsters.AddAsync(monster, cancellationToken);
    }

    public async Task InsertAsync(IEnumerable<Monster> monsters, CancellationToken cancellationToken)
    {
        await context.Monsters.AddRangeAsync(monsters, cancellationToken);
    }

    public async ValueTask<Monster?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Monsters.FindAsync(id, cancellationToken);
    }

    public void Remove(Monster monster)
    {
        context.Remove(monster);
    }
}
