using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Battles;
using SharedKernel;

namespace Application.Battles.Remove;

public sealed record RemoveBattleCommand(Guid Id) : ICommand;

internal sealed class RemoveBattleCommandHandler(
    IBattleRepository battleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveBattleCommand>
{
    public async Task<Result> Handle(RemoveBattleCommand command, CancellationToken cancellationToken)
    {
        var battle = await battleRepository.FindAsync(command.Id, cancellationToken);

        if (battle == null)
            return Result.Failure<BattleResponse>(BattleErrors.NotFound(command.Id));

        battleRepository.Remove(battle);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
