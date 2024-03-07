using Application.Abstractions.Messaging;
using Application.Battles;
using Application.Battles.GetById;
using Domain.Battles;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Battles;

internal sealed class GetBattleByIdQueryHandler(BattleOfMonstersContext context)
    : IQueryHandler<GetBattleByIdQuery, BattleResponse>
{
    public async Task<Result<BattleResponse>> Handle(
        GetBattleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var battle = await context.Battles
            .Where(m => m.Id == query.BattleId)
            .Select(m => new BattleResponse
            {
                Id = m.Id,
                MonsterA = m.MonsterA,
                MonsterB = m.MonsterB,
                MonsterWinnerId = m.MonsterWinnerId
            })
        .FirstOrDefaultAsync(cancellationToken);

        if (battle == null) return Result.Failure<BattleResponse>(BattleErrors.NotFound(query.BattleId));
        return battle;
    }
}
