using API.Test.Fixtures;
using Domain.Monsters;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace API.Test
{
    [TestCaseOrderer("API.Test.AlphabeticalOrderer", "API.Test")]
    public class MonsterEndpointsIntegrationTests : IClassFixture<ApiApplicationFixture>
    {
        private readonly ApiApplication _application;
        private readonly Monster _monster;

        public MonsterEndpointsIntegrationTests(ApiApplicationFixture apiApplicationFixture)
        {
            _application = apiApplicationFixture.Application;
            _monster = apiApplicationFixture.Monster;
        }

        [Fact]
        public async Task A_PostCreateMonster()
        {
            using var client = _application.CreateClient();

            var response = await client.PostAsJsonAsync("/monsters", _monster);
            var monsterResponse = await HttpClientHelper.ReadJsonResponser<Monster>(response);
            _monster.Should().BeEquivalentTo(
                    monsterResponse,
                    options => options.Excluding(ex => ex.Id)
            );
        }

        [Fact]
        public async Task B_GetReturnsListOfMonsters()
        {
            using var client = _application.CreateClient();

            var response = await client.GetAsync("/monsters");
            var listMonsters = await HttpClientHelper.ReadJsonResponser<List<Monster>>(response);
            listMonsters.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task C_PutUpdateMonster()
        {
            using var client = _application.CreateClient();

            _monster.Name = "TestUpdate";

            var response = await client.PutAsJsonAsync($"/monsters/{_monster.Id}", _monster);
            var monsterResponse = await HttpClientHelper.ReadJsonResponser<Monster>(response);
            _monster.Should().BeEquivalentTo(monsterResponse);
        }

        [Fact]
        public async Task D_GetReturnsOneMonsterById()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"/monsters/{_monster.Id}");
            var monsterResponse = await HttpClientHelper.ReadJsonResponser<Monster>(response);
            _monster.Should().BeEquivalentTo(monsterResponse);
        }

        [Fact]
        public async Task E_DeleteRemoveMonster()
        {
            using var client = _application.CreateClient();
            var response = await client.DeleteAsync($"/monsters/{_monster.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task F_GetReturnsEmptyList()
        {
            using var client = _application.CreateClient();

            var response = await client.GetAsync("/monsters");
            var listMonsters = await HttpClientHelper.ReadJsonResponser<List<Monster>>(response);
            listMonsters.Should().BeNullOrEmpty();
        }
    }
}
