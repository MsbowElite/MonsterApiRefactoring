using SharedKernel;

namespace Domain.Monsters;

public static class MonsterErrors
{
    public static Error NotFound(Guid monsterId) => Error.NotFound(
        "Monsters.NotFound",
        $"The monster with the Id = '{monsterId}' was not found");
}
