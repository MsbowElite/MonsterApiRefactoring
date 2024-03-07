using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Battles;
using Domain.Monsters;
using SharedKernel;

namespace Application.Monsters.Remove;

public sealed record RemoveMonsterCommand(Guid Id) : ICommand;

internal sealed class RemoveBattleCommandHandler(
    IMonsterRepository monsterRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveMonsterCommand>
{
    public async Task<Result> Handle(RemoveMonsterCommand command, CancellationToken cancellationToken)
    {
        var monster = await monsterRepository.FindAsync(command.Id, cancellationToken);

        if (monster == null)
            return Result.Failure<MonsterResponse>(BattleErrors.NotFound(command.Id));

        monsterRepository.Remove(monster);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
