using Application.Abstractions.Messaging;

namespace Application.Battles.GetById
{
    public sealed record GetBattleByIdQuery(Guid BattleId) : IQuery<BattleResponse> { }
}
