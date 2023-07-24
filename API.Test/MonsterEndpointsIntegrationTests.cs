using API.Test.Fixtures;
using FluentAssertions;
using Lib.Repository.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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

        /// <summary>
        /// PostCreateMonster
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task A()
        {
            using var client = _application.CreateClient();

            var response = await client.PostAsJsonAsync("/monsters", _monster);
            var monsterResponse = await HttpClientHelper.ReadJsonResponser<Monster>(response);
            _monster.Should().BeEquivalentTo(
                    monsterResponse,
                    options => options.Excluding(ex => ex.Id)
            );

            _monster.Id = monsterResponse.Id;
        }

        /// <summary>
        /// GetReturnsListOfMonsters
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task B()
        {
            using var client = _application.CreateClient();

            var response = await client.GetAsync("/monsters");
            var listMonsters = await HttpClientHelper.ReadJsonResponser<List<Monster>>(response);
            listMonsters.Should().HaveCountGreaterThan(0);
        }

        /// <summary>
        /// PutUpdateMonster
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task C()
        {
            using var client = _application.CreateClient();

            _monster.Name = "TestUpdate";

            var response = await client.PutAsJsonAsync($"/monsters/{_monster.Id}", _monster);
            var monsterResponse = await HttpClientHelper.ReadJsonResponser<Monster>(response);
            _monster.Should().BeEquivalentTo(monsterResponse);
        }

        /// <summary>
        /// GetReturnsOneMonsterById
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task D()
        {
            using var client = _application.CreateClient();
            var response = await client.GetAsync($"/monsters/{_monster.Id}");
            var monsterResponse = await HttpClientHelper.ReadJsonResponser<Monster>(response);
            _monster.Should().BeEquivalentTo(monsterResponse);
        }

        /// <summary>
        /// DeleteRemoveMonster
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task E()
        {
            using var client = _application.CreateClient();
            var response = await client.DeleteAsync($"/monsters/{_monster.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// GetReturnsEmptyList
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task F()
        {
            using var client = _application.CreateClient();

            var response = await client.GetAsync("/monsters");
            var listMonsters = await HttpClientHelper.ReadJsonResponser<List<Monster>>(response);
            listMonsters.Should().BeNullOrEmpty();
        }
    }
}
