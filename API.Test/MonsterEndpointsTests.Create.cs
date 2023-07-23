using API.Endpoints;
using Lib.Repository.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentAssertions;
using API.Test.Fixtures;
using FluentValidation;

namespace API.Test
{
    public partial class MonsterEndpointsTests
    {
        [Fact]
        public async Task Post_OnSuccess_CreateMonster()
        {
            Monster m = new()
            {
                Name = "Monster Test",
                Attack = 50,
                Defense = 40,
                Hp = 80,
                Speed = 60,
                ImageUrl = ""
            };
            string BaseRoute = "monsters";

            _validator
                .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repository
                .Setup(x => x.AddAsync(m));

            _repository
                .Setup(x => x.UnitOfWork.Commit())
                .ReturnsAsync(true);

            IResult result = await MonsterEndpoints.CreateMonsterAsync(m, _repository.Object, _validator.Object);
            result.Should().BeOfType<Created<Monster>>();
            var created = result as Created<Monster>;
            Assert.Equal(created.Location, $"/{BaseRoute}/{m.Id}");
        }

        [Fact]
        public async Task Post_OnNoMonsterFailureValidation_Returns400()
        {
            const int id = 123;
            string failureText = "Cannot be empty";

            Monster m = new()
            {
                Name = null,
                Attack = 50,
                Defense = 40,
                Hp = 80,
                Speed = 60,
                ImageUrl = ""
            };

            _validator
                .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new ValidationFailure[] { new ValidationFailure(nameof(m.Name), failureText) }));

            IResult result = await MonsterEndpoints.CreateMonsterAsync(m, _repository.Object, _validator.Object);
            result.Should().BeOfType<BadRequest<List<ValidationFailure>>>();
            var badRequest = result as BadRequest<List<ValidationFailure>>;
            var firstErrorResult = badRequest.Value.First();
            Assert.Equal(nameof(m.Name), firstErrorResult.PropertyName);
            Assert.Equal(failureText, firstErrorResult.ErrorMessage);
        }

        [Fact]
        public async Task Post_OnDuplicatedId_Returns400()
        {
            string failureText = "A monster with this Id already exists";
            Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

            _validator
                .Setup(x => x.ValidateAsync(monsters[0], It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _repository
               .Setup(x => x.AddAsync(monsters[0]));

            _repository
                .Setup(x => x.UnitOfWork.Commit())
                .ReturnsAsync(false);

            IResult result = await MonsterEndpoints.CreateMonsterAsync(monsters[0], _repository.Object, _validator.Object);
            result.Should().BeOfType<BadRequest<List<ValidationFailure>>>();
            var badRequest = result as BadRequest<List<ValidationFailure>>;
            var firstErrorResult = badRequest.Value.First();
            Assert.Equal(nameof(Monster.Id), firstErrorResult.PropertyName);
            Assert.Equal(failureText, firstErrorResult.ErrorMessage);
        }
    }
}
