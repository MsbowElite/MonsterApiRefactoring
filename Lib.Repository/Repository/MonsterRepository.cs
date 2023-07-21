using Lib.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Lib.Repository.Repository;

public class MonsterRepository : IMonsterRepository
{
    private readonly BattleOfMonstersContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public MonsterRepository(BattleOfMonstersContext context)
    {
        _context = context;
    }

    public ValueTask<EntityEntry<Monster>> AddAsync(Monster monster)
    {
        return _context.Set<Monster>().AddAsync(monster);
    }

    public Task AddAsync(IEnumerable<Monster> monsters)
    {
        return _context.Set<Monster>().AddRangeAsync(monsters);
    }

    public ValueTask<Monster?> FindAsync(int? id)
    {
        return _context.Set<Monster>().FindAsync(id);
    }

    public async Task<Monster[]> GetAllAsync()
    {
        return await _context.Set<Monster>().ToArrayAsync();
    }

    public void Remove(Monster monster)
    {
        if (monster != null)
        {
            _context.Set<Monster>().Remove(monster);
        }
    }

    public void Update(Monster oldMonster, Monster newMonster)
    {
        if (oldMonster != null)
        {
            _context.Entry(oldMonster).CurrentValues.SetValues(newMonster);
        }
    }
}