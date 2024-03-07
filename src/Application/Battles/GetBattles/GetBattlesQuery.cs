using Application.Abstractions.Messaging;

namespace Application.Battles.GetBattles;

public sealed record GetBattlesQuery() : IQuery<BattleResponse[]>;
