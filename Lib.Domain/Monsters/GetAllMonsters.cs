using Lib.Repository.Entities;
using Lib.Repository.Repository;
using MediatR;

namespace Lib.Domain.Monsters;

public record GetAllMonstersQuery : IRequest<IEnumerable<Monster>>;

public class GetAllMonstersHandler : IRequestHandler<GetAllMonstersQuery, IEnumerable<Monster>>
{
    private readonly IMonsterRepository _monsterRepository;

    public GetAllMonstersHandler(IMonsterRepository monsterRepository)
    {
        _monsterRepository = monsterRepository;
    }

    public async Task<IEnumerable<Monster>> Handle(GetAllMonstersQuery request, CancellationToken cancellationToken)
    {
        return await _monsterRepository.GetAllAsync();
    }
}
