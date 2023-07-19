using FluentValidation;
using Lib.Repository.Entities;

namespace API.Validators
{
    public class BattleValidator : AbstractValidator<Battle>
    {
        public BattleValidator()
        {
            RuleFor(battle => battle.MonsterA)
                .NotNull().NotEmpty();
            RuleFor(battle => battle.MonsterB)
                .NotNull().NotEmpty();
        }
    }
}
