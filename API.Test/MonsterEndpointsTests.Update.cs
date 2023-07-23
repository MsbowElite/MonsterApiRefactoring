using API.Endpoints;
using API.Test.Fixtures;
using FluentAssertions;
using FluentValidation.Results;
using Lib.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Collections.Generic;

namespace API.Test;

public partial class MonsterEndpointsTests
{
    [Fact]
    public async Task Put_OnSuccess_UpdateMonster()
    {
        const int id = 1;
        Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

        Monster m = new()
        {
            Name = "Monster Update"
        };


        _validator
            .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repository
            .Setup(x => x.FindAsync(id))
            .ReturnsAsync(monsters[0]);

        _repository
           .Setup(x => x.Update(monsters[0], m));

        _repository
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(true);

        IResult result = await MonsterEndpoints.UpdateMonsterAsync(id, m, _repository.Object, _validator.Object);
        result.Should().BeOfType<Ok<Monster>>();
    }

    [Fact]
    public async Task Put_OnDatabaseOffline_UpdateMonster()
    {
        const int id = 1;
        Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

        Monster m = new()
        {
            Name = "Monster Update"
        };


        _validator
            .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repository
            .Setup(x => x.FindAsync(id))
            .ReturnsAsync(monsters[0]);

        _repository
           .Setup(x => x.Update(monsters[0], m));

        _repository
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(false);

        IResult result = await MonsterEndpoints.UpdateMonsterAsync(id, m, _repository.Object, _validator.Object);
        result.Should().BeOfType<UnprocessableEntity<Monster>>();
    }

    [Fact]
    public async Task Put_OnNoMonsterFound_Returns404()
    {
        const int id = 123;

        Monster m = new()
        {
            Name = "Monster Update"
        };

        _validator
            .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repository
        .Setup(x => x.Update(null, m));

        IResult result = await MonsterEndpoints.UpdateMonsterAsync(id, m, _repository.Object, _validator.Object);
        result.Should().BeOfType<NotFound<string>>();
        var notFound = result as NotFound<string>;
        Assert.Equal($"The monster with ID = {id} not found.", notFound.Value);
    }

    [Fact]
    public async Task Put_OnNoMonsterFailureValidation_Returns400()
    {
        const int id = 123;
        string failureText = "Cannot be empty";

        Monster m = new()
        {
            Name = null
        };

        _validator
            .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new ValidationFailure[] { new ValidationFailure(nameof(m.Name), failureText) }));

        IResult result = await MonsterEndpoints.UpdateMonsterAsync(id, m, _repository.Object, _validator.Object);
        result.Should().BeOfType<BadRequest<List<ValidationFailure>>>();
        var badRequest = result as BadRequest<List<ValidationFailure>>;
        var firstErrorResult = badRequest.Value.First();
        Assert.Equal(nameof(m.Name), firstErrorResult.PropertyName);
        Assert.Equal(failureText, firstErrorResult.ErrorMessage);
    }
}

