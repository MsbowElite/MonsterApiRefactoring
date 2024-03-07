namespace Application.Monsters.Create;

public sealed record CreateMonsterRequest(
    string Name,
    int Attack,
    int Defense,
    int Hp,
    string ImageUrl,
    int Speed);
