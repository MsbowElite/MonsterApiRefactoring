using Application.Monsters.GetById;
using Application.Monsters.Update;
using Domain.Battles;
using Domain.Monsters;
using MediatR;
using SharedKernel;

namespace Application.Battles.Create;

internal class CreatedDomainEventHandler(
    ISender sender,
    IMonsterRepository monsterRepository) : INotificationHandler<CreatedDomainEvent>
{
    public async Task Handle(
        CreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var monster = await monsterRepository.FindAsync(notification.monsterId, cancellationToken);
        if (monster is not null)
        {
            monster.Hp -= notification.damage;

            _ = await sender.Send(new UpdateMonsterCommand(
                monster.Id,
                monster.Name,
                monster.Attack,
                monster.Defense,
                monster.Hp,
                monster.ImageUrl,
                monster.Speed), cancellationToken);
        }
    }
}
