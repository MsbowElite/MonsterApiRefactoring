namespace Application.Battles.Create;

public sealed record CreateBattleRequest(Guid MonsterA, Guid MonsterB, Guid MonsterWinner);
