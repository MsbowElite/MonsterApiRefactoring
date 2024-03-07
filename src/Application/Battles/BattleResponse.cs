namespace Application.Battles;
public sealed record BattleResponse
{
    public Guid Id { get; init; }
    public Guid MonsterA { get; init; }
    public Guid MonsterB { get; init; }
    public Guid MonsterWinnerId { get; init; }
}
