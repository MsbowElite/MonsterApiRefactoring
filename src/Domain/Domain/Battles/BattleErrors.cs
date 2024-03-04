using SharedKernel;

namespace Domain.Battles;

public static class BattleErrors
{
    public static Error NotFound(Guid battleId) => Error.NotFound(
        "Monsters.NotFound",
        $"The monster with the Id = '{battleId}' was not found");
}
