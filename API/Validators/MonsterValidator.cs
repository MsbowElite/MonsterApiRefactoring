using FluentValidation;
using Lib.Repository.Entities;

namespace API.Validators
{
    public class MonsterValidator : AbstractValidator<Monster>
    {
        public MonsterValidator()
        {
            RuleFor(monster => monster.Name)
                .NotEmpty();
        }
    }
}
