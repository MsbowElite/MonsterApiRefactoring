using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Monsters;
using SharedKernel;

namespace Application.Monsters.Create;

public sealed record CreateMonsterCommand(
    string Name,
    int Attack,
    int Defense,
    int Hp,
    string ImageUrl,
    int Speed) : ICommand<Guid>;

internal sealed class CreateMonsterCommandHandler(
    IMonsterRepository monsterRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMonsterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
    CreateMonsterCommand request,
    CancellationToken cancellationToken)
    {
        var monster = new Monster(
            Guid.NewGuid(),
            request.Name,
            request.Attack,
            request.Defense,
            request.Hp,
            request.ImageUrl,
            request.Speed
            );

        await monsterRepository.InsertAsync(monster, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return monster.Id;
    }
}
