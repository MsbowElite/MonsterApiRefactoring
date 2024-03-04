//using API.Endpoints;
//using API.Test.Fixtures;
//using Domain.Monsters;
//using FluentAssertions;
//using FluentValidation.Results;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Moq;

//namespace API.Test
//{
//    public partial class MonsterEndpointsTests
//    {
//        [Fact]
//        public async Task Post_OnSuccess_CreateMonster()
//        {
//            Monster m = new(
//                Guid.NewGuid(),
//                "Monster Test",
//                50,
//                40,
//                80,
//                "",
//                60
//            );
//            string BaseRoute = "monsters";

//            //_validator
//            //    .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
//            //    .ReturnsAsync(new ValidationResult());

//            _sender
//                .Setup(x => x.AddAsync(m));

//            _sender
//                .Setup(x => x.UnitOfWork.Commit())
//                .ReturnsAsync(true);

//            IResult result = await MonsterEndpoints.CreateMonsterAsync(m, _sender.Object, _validator.Object);
//            result.Should().BeOfType<Created<Monster>>();
//            var created = result as Created<Monster>;
//            Assert.Equal(created.Location, $"/{BaseRoute}/{m.Id}");
//        }

//        [Fact]
//        public async Task Post_OnNoMonsterFailureValidation_Returns400()
//        {
//            const int id = 123;
//            string failureText = "Cannot be empty";

//            Monster m = new(
//                Guid.NewGuid(),
//                null,
//                50,
//                40,
//                80,
//                "",
//                60
//            );

//            _validator
//                .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
//                .ReturnsAsync(new ValidationResult(new ValidationFailure[] { new ValidationFailure(nameof(m.Name), failureText) }));

//            IResult result = await MonsterEndpoints.CreateMonsterAsync(m, _sender.Object, _validator.Object);
//            result.Should().BeOfType<BadRequest<List<ValidationFailure>>>();
//            var badRequest = result as BadRequest<List<ValidationFailure>>;
//            var firstErrorResult = badRequest.Value.First();
//            Assert.Equal(nameof(m.Name), firstErrorResult.PropertyName);
//            Assert.Equal(failureText, firstErrorResult.ErrorMessage);
//        }

//        [Fact]
//        public async Task Post_OnDuplicatedId_Returns400()
//        {
//            string failureText = "A monster with this Id already exists";
//            Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

//            _validator
//                .Setup(x => x.ValidateAsync(monsters[0], It.IsAny<CancellationToken>()))
//                .ReturnsAsync(new ValidationResult());

//            _sender
//               .Setup(x => x.AddAsync(monsters[0]));

//            _sender
//                .Setup(x => x.UnitOfWork.Commit())
//                .ReturnsAsync(false);

//            IResult result = await MonsterEndpoints.CreateMonsterAsync(monsters[0], _sender.Object, _validator.Object);
//            result.Should().BeOfType<BadRequest<List<ValidationFailure>>>();
//            var badRequest = result as BadRequest<List<ValidationFailure>>;
//            var firstErrorResult = badRequest.Value.First();
//            Assert.Equal(nameof(Monster.Id), firstErrorResult.PropertyName);
//            Assert.Equal(failureText, firstErrorResult.ErrorMessage);
//        }
//    }
//}
