using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Battles;
using SharedKernel;

namespace Application.Battles.Create
{
    public sealed record CreateBattleCommand(
        Guid MonsterA,
        Guid MonsterB,
        Guid MonsterWinner
        ) : ICommand<Guid>;

    internal sealed class CreateBattleCommandHandler(
        IBattleRepository battleRepository,
        IUnitOfWork unitOfWork
        ) : ICommandHandler<CreateBattleCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateBattleCommand command, CancellationToken cancellationToken)
        {
            var battle = new Battle(
                Guid.NewGuid(),
                command.MonsterA,
                command.MonsterB,
                command.MonsterWinner);

            await battleRepository.InsertAsync(battle, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return battle.Id;
        }
    }
}
