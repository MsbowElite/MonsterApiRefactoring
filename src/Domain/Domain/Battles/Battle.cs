using Domain.Monsters;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Battles;

public sealed class Battle : Entity
{
    public Battle(Guid id,
                  Guid monsterA,
                  Guid monsterB,
                  Guid monsterWinnerId) : base(id)
    {
        MonsterA = monsterA;
        MonsterB = monsterB;
        MonsterWinnerId = monsterWinnerId;
    }

    private Battle() { }

    public Guid MonsterA { get; set; }
    public Guid MonsterB { get; set; }
    public Guid MonsterWinnerId { get; set; }

    public Monster? MonsterARelation { get; set; }
    public Monster? MonsterBRelation { get; set; }
    public Monster? WinnerRelation { get; set; }
}
