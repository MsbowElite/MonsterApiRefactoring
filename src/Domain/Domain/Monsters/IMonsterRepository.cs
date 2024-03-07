namespace Domain.Monsters;

public interface IMonsterRepository
{
    Task InsertAsync(Monster monster, CancellationToken cancellationToken);
    Task InsertAsync(IEnumerable<Monster> monsters, CancellationToken cancellationToken);
    public ValueTask<Monster?> FindAsync(Guid id, CancellationToken cancellationToken);
    public void Remove(Monster monster);
}
