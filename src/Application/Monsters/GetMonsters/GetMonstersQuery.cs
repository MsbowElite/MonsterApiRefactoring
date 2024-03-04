using Application.Abstractions.Messaging;

namespace Application.Monsters.GetMonsters;

public sealed record GetMonsersQuery() : IQuery<MonsterResponse[]>;
