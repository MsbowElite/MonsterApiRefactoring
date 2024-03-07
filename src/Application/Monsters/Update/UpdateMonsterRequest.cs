namespace Application.Monsters.Update;

public sealed record UpdateMonsterRequest(
    string Name,
    int Attack,
    int Defense,
    int Hp,
    string ImageUrl,
    int Speed);
