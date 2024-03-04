using Domain.Battles;

namespace API.Test.Fixtures;

public static class BattlesFixture
{
    public static IEnumerable<Battle> GetBattlesMock()
    {
        var MonsterIdA = Guid.NewGuid();
        return new[]
        {
            new Battle(
            
                Guid.NewGuid(),
                MonsterIdA,
                Guid.NewGuid(),
                MonsterIdA
            )
        };
    }
}