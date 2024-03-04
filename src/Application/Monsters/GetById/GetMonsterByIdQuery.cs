using Application.Abstractions.Caching;

namespace Application.Monsters.GetById;

public sealed record GetMonserByIdQuery(Guid MonsterId) : ICachedQuery<MonsterResponse>
{
    public string CacheKey => $"monster-by-id-{MonsterId}";

    public TimeSpan? Expiration => null;
}
