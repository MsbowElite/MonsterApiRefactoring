using Application.Abstractions.Messaging;
using Application.Monsters;
using Application.Monsters.GetMonsters;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Queries.Monsters;

internal sealed class GetMonstersQueryHandler(BattleOfMonstersReadDbContext context)
    : IQueryHandler<GetMonsersQuery, MonsterResponse[]>
{
    public async Task<Result<MonsterResponse[]>> Handle(
        GetMonsersQuery query,
        CancellationToken cancellationToken)
    {
        return await context.Monsters
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
            .ToArrayAsync(cancellationToken);
    }
}
