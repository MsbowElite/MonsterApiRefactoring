
namespace Application.Monsters
{
    public sealed record MonsterResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int Attack { get; init; }
        public int Defense { get; init; }
        public int Hp { get; init; }
        public string ImageUrl { get; init; }
        public int Speed { get; init; }
    }
}
