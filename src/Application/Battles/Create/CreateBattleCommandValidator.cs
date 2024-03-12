using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Battles.Create
{
    internal sealed class CreateBattleCommandValidator : AbstractValidator<CreateBattleCommand>
    {
        public CreateBattleCommandValidator() 
        {
            RuleFor(battle => battle.MonsterA)
                .NotNull().NotEmpty();
            RuleFor(battle => battle.MonsterB)
                .NotNull().NotEmpty();
        }
    }
}
