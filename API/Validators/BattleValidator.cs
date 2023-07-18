using Lib.Repository.Entities;

namespace API.Validators
{
    public class BattleValidator
    {
        public static bool IsValid(Battle battle)
        {
            if (battle.MonsterA is null || battle.MonsterB is null) return false;
            return true;
        }
    }
}
