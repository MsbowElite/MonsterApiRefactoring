using Application.Monsters;
using Domain.Monsters;

namespace API.Test.Fixtures;

public static class MonsterFixture
{
    public static IEnumerable<Monster> GetMonstersMock()
    {
        return
        [
            new Monster(
                Guid.NewGuid(),
                "monster-1",
                40,
                20,
                50,
                "",
                80
            ),
            new Monster(
                Guid.NewGuid(),
                "monster-2",
                40,
                20,
                50,
                "",
                80
            ),
            new Monster(

                Guid.NewGuid(),
                "monster-3",
                40,
                20,
                50,
                "",
                80
            ),
            new Monster(
                Guid.NewGuid(),
                "monster-4",
                40,
                20,
                50,
                "",
                80
            ),
            new Monster(
                Guid.NewGuid(),
                "monster-5",
                40,
                20,
                50,
                "",
                80
            ),
            new Monster(
                Guid.NewGuid(),
                "monster-6",
                40,
                20,
                50,
                "",
                80
            ),
            new Monster(
                Guid.NewGuid(),
                "monster-7",
                40,
                20,
                50,
                "",
                80
            )
        ];
    }
    public static IEnumerable<MonsterResponse> GetMonstersResponseMock()
    {
        return
        [
            new MonsterResponse
            {
                Id = Guid.NewGuid(),
                Name = "monster-1",
                Attack = 40,
                Defense = 20,
                Hp = 50,
                Speed = 80,
                ImageUrl = ""
            },
            new MonsterResponse
            {
                Id = Guid.NewGuid(),
                Name = "monster-2",
                Attack = 70,
                Defense = 20,
                Hp = 100,
                Speed = 40,
                ImageUrl = ""
            },
            new MonsterResponse
            {
                Id = Guid.NewGuid(),
                Name = "monster-3",
                Attack = 40,
                Defense = 20,
                Hp = 50,
                Speed = 10,
                ImageUrl = ""
            },
            new MonsterResponse
            {
                Id = Guid.NewGuid(),
                Name = "monster-4",
                Attack = 70,
                Defense = 20,
                Hp = 50,
                Speed = 40,
                ImageUrl = ""
            },
            new MonsterResponse
            {
                Id = Guid.NewGuid(),
                Name = "monster-5",
                Attack = 40,
                Defense = 20,
                Hp = 100,
                Speed = 40,
                ImageUrl = ""
            },
            new MonsterResponse
            {
                Id = Guid.NewGuid(),
                Name = "monster-6",
                Attack = 10,
                Defense = 10,
                Hp = 100,
                Speed = 80,
                ImageUrl = ""
            },
            new MonsterResponse
            {
                Id = Guid.NewGuid(),
                Name = "monster-7",
                Attack = 60,
                Defense = 10,
                Hp = 150,
                Speed = 40,
                ImageUrl = ""
            }
        ];
    }
}