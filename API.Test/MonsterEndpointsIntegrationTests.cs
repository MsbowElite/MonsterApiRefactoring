using API.Test.Fixtures;
using FluentAssertions;
using Lib.Repository.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Text.Json;

namespace API.Test
{
    public class MonsterEndpointsIntegrationTests : IClassFixture<ApiApplicationFixture>
    {
        private readonly ApiApplication _application;

        public MonsterEndpointsIntegrationTests(ApiApplicationFixture apiApplicationFixture)
        {
            _application = apiApplicationFixture.Application;
        }

        [Fact]
        public async Task A_Post_OnSuccess_CreateMonster()
        {
            using var client = _application.CreateClient();

            Monster m = new()
            {
                Name = "Monster Test",
                Attack = 50,
                Defense = 40,
                Hp = 80,
                Speed = 60,
                ImageUrl = ""
            };

            var response = await client.PostAsJsonAsync("/monsters", m);
            m.Should().BeEquivalentTo(
                    await HttpClientHelper.ReadJsonResponser<Monster>(response),
                    options => options.Excluding(ex => ex.Id)
            );
        }

        [Fact]
        public async Task B_Get_OnSuccess_ReturnsListOfMonsters()
        {
            using var client = _application.CreateClient();

            var response = await client.GetAsync("/monsters");
            var listMonsters = await HttpClientHelper.ReadJsonResponser<List<Monster>>(response);
            listMonsters.Should().HaveCountGreaterThan(0);
        }
    }
}
