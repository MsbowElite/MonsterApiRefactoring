using Domain.Monsters;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Battles;

public class Battle(
    Guid id,
    Guid monsterIdA,
    Guid monsterIdB,
    Guid monsterWinnerId) : Entity(id)
{
    public Guid MonsterA { get; set; } = monsterIdA;
    public Guid MonsterB { get; set; } = monsterIdB;
    public Guid MonsterWinnerId { get; set; } = monsterWinnerId;

    public Monster? MonsterARelation { get; set; }
    public Monster? MonsterBRelation { get; set; }
    public Monster? WinnerRelation { get; set; }
}
