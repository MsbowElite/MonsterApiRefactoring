using Application.Abstractions.Messaging;
using Application.Battles;
using Application.Battles.GetBattles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Battles;

internal sealed class GetBattlesQueryHandler(BattleOfMonstersContext context)
    : IQueryHandler<GetBattlesQuery, BattleResponse[]>
{
    public async Task<Result<BattleResponse[]>> Handle(
        GetBattlesQuery query,
        CancellationToken cancellationToken)
    {
        return await context.Battles
       .Select(m => new BattleResponse
       {
           Id = m.Id,
           MonsterA = m.MonsterA,
           MonsterB = m.MonsterB,
           MonsterWinnerId = m.MonsterWinnerId
       }).ToArrayAsync(cancellationToken);
    }
}
