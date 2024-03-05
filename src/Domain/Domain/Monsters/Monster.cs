using SharedKernel;

namespace Domain.Monsters;

public class Monster : Entity
{
    public Monster(Guid id,
                   string name,
                   int attack,
                   int defense,
                   int hp,
                   string imageUrl,
                   int speed) : base(id)
    {
        Name = name;
        Attack = attack;
        Defense = defense;
        Hp = hp;
        ImageUrl = imageUrl;
        Speed = speed;
    }

    private Monster() { }

    public string Name { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Hp { get; set; }
    public string ImageUrl { get; set; }
    public int Speed { get; set; }
}
