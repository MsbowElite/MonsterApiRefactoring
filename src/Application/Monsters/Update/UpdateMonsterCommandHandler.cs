using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Monsters;
using SharedKernel;

namespace Application.Monsters.Update;

public sealed record UpdateMonsterCommand(
    Guid Id,
    string Name,
    int Attack,
    int Defense,
    int Hp,
    string ImageUrl,
    int Speed) : ICommand<MonsterResponse>;

internal sealed class UpdateMonsterCommandHandler(
    IMonsterRepository monsterRepository,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateMonsterCommand, MonsterResponse>
{
    public async Task<Result<MonsterResponse>> Handle(UpdateMonsterCommand command, CancellationToken cancellationToken)
    {
        var monster = await monsterRepository.FindAsync(command.Id, cancellationToken);

        if (monster == null)
            return Result.Failure<MonsterResponse>(MonsterErrors.NotFound(command.Id));

        monster.Name = command.Name;
        monster.Attack = command.Attack;
        monster.Defense = command.Defense;
        monster.Hp = command.Hp;
        monster.Speed = command.Speed;
        monster.ImageUrl = command.ImageUrl;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new MonsterResponse
        {
            Id = monster.Id,
            Name = monster.Name,
            Attack = monster.Attack,
            Defense = monster.Defense,
            Hp = monster.Hp,
            ImageUrl = monster.ImageUrl,
            Speed = monster.Speed
        };
    }
}

