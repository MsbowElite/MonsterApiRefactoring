using SharedKernel;

namespace Domain.Monsters;

public class Monster(
    Guid id,
    string name,
    int attack,
    int defense,
    int hp,
    string imageUrl,
    int speed) : Entity(id)
{
    public string Name { get; set; } = name;
    public int Attack { get; set; } = attack;
    public int Defense { get; set; } = defense;
    public int Hp { get; set; } = hp;
    public string ImageUrl { get; set; } = imageUrl;
    public int Speed { get; set; } = speed;
}
