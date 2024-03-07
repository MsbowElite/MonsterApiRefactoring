using Application.Abstractions.Messaging;
using Application.Monsters;
using Application.Monsters.GetById;
using Domain.Monsters;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Monsters;

internal sealed class GetMonsterByIdQueryHandler(BattleOfMonstersContext context)
    : IQueryHandler<GetMonserByIdQuery, MonsterResponse>
{
    public async Task<Result<MonsterResponse>> Handle(
        GetMonserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var monster = await context.Monsters
            .Where(m => m.Id == query.MonsterId)
            .Select(m => new MonsterResponse
            {
                Id = m.Id,
                Attack = m.Attack,
                Defense = m.Defense,
                Hp = m.Hp,
                ImageUrl = m.ImageUrl,
                Name = m.Name,
                Speed = m.Speed
            })
        .FirstOrDefaultAsync(cancellationToken);

        if (monster == null) return Result.Failure<MonsterResponse>(MonsterErrors.NotFound(query.MonsterId));
        return monster;
    }
}
