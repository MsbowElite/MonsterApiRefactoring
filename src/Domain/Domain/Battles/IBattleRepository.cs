namespace Domain.Battles
{
    public interface IBattleRepository
    {
        public Task InsertAsync(Battle battle, CancellationToken cancellationToken);
        public ValueTask<Battle?> FindAsync(Guid id, CancellationToken cancellationToken);
        public void Remove(Battle battle);
    }
}
