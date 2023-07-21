using Lib.Repository.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;

namespace Lib.Repository.Repository;

public interface IMonsterRepository : IRepository
{
    public ValueTask<EntityEntry<Monster>> AddAsync(Monster monster);
    public Task AddAsync(IEnumerable<Monster> monsters);
    public ValueTask<Monster?> FindAsync(int? id);
    public Task<Monster[]> GetAllAsync();
    public void Remove(Monster monster);
    public void Update(Monster oldMonster, Monster newMonster);
}