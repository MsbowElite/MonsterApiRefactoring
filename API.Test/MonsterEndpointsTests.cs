using System.Diagnostics;
using API.Endpoints;
using API.Test.Fixtures;
using API.Validators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test;

public class MonsterEndpointsTests
{
    private readonly Mock<IMonsterRepository> _repository;
    private readonly Mock<IValidator<Monster>> _validator;

    public MonsterEndpointsTests()
    {
        _repository = new Mock<IMonsterRepository>();
        _validator = new Mock<IValidator<Monster>>();
    }
    
    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfMonsters()
    {
        Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

        this._repository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(monsters);
        
        IResult result = await MonsterEndpoints.GetAllMonstersAsync(_repository.Object);
        OkObjectResult objectResults = (OkObjectResult) result;
        objectResults?.Value.Should().BeOfType<Monster[]>();
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsOneMonsterById()
    {
        const int id = 1;
        Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

        Monster monster = monsters[0];
        this._repository
            .Setup(x => x.FindAsync(id))
            .ReturnsAsync(monster);

        IResult result = await MonsterEndpoints.GetMonsterByIdAsync(id, _repository.Object);
        result.Should().BeOfType<Ok<Monster>>();
    }

    [Fact]
    public async Task Get_OnNoMonsterFound_Returns404()
    {
        const int id = 123;

        this._repository
            .Setup(x => x.FindAsync(id))
            .ReturnsAsync(() => null);

        IResult result = await MonsterEndpoints.GetMonsterByIdAsync(id, _repository.Object);
        NotFoundObjectResult objectResults = (NotFoundObjectResult)result;
        result.Should().BeOfType<NotFoundObjectResult>();
        Assert.Equal($"The monster with ID = {id} not found.", objectResults.Value);
    }

    [Fact]
    public async Task Post_OnSuccess_CreateMonster()
    {
        Monster m = new Monster()
        {
            Name = "Monster Test",
            Attack = 50,
            Defense = 40,
            Hp = 80,
            Speed = 60,
            ImageUrl = ""
        };

        _repository
            .Setup(x => x.AddAsync(m));
        _validator
            .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        IResult result = await MonsterEndpoints.CreateMonsterAsync(m, _repository.Object, _validator.Object);
        OkObjectResult objectResults = (OkObjectResult)result;
        objectResults?.Value.Should().BeOfType<Monster>();
    }

    [Fact]
    public async Task Put_OnSuccess_UpdateMonster()
    {
        const int id = 1;
        Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

        Monster m = new Monster()
        {
            Name = "Monster Update"
        };

        _repository
           .Setup(x => x.Update(monsters[0], m));
        _validator
            .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        IResult result = await MonsterEndpoints.UpdateMonsterAsync(id, m, _repository.Object, _validator.Object);
        OkResult objectResults = (OkResult)result;
        objectResults.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Put_OnNoMonsterFound_Returns404()
    {
        const int id = 123;

        Monster m = new Monster()
        {
            Name = "Monster Update"
        };

        _repository
           .Setup(x => x.Update(null, m));
        _validator
            .Setup(x => x.ValidateAsync(m, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        IResult result = await MonsterEndpoints.UpdateMonsterAsync(id, m, _repository.Object, _validator.Object);
        NotFoundObjectResult objectResults = (NotFoundObjectResult)result;
        result.Should().BeOfType<NotFoundObjectResult>();
        Assert.Equal($"The monster with ID = {id} not found.", objectResults.Value);
    }


    [Fact]
    public async Task Delete_OnSuccess_RemoveMonster()
    {
        const int id = 1;
        Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

        _repository
            .Setup(x => x.FindAsync(id))
            .ReturnsAsync(() => monsters[0]);

        _repository
           .Setup(x => x.Remove(monsters[0]));

        _repository
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(true);

        IResult result = await MonsterEndpoints.DeleteMonsterAsync(id, _repository.Object);
        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task Delete_OnNoMonsterFound_Returns404()
    {
        const int id = 123;

        _repository
           .Setup(x => x.Remove(null));

        _repository
            .Setup(x => x.FindAsync(id))
            .ReturnsAsync(() => null);

        _repository
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(false);
        
        var result = await MonsterEndpoints.DeleteMonsterAsync(id, _repository.Object);
        result.Should().BeOfType<NotFound<string>>();
        var notFound = result as NotFound<string>;
        Assert.Equal($"The monster with ID = {id} not found.", notFound.Value);
    }

    [Fact]
    public async Task Post_OnSuccess_ImportCsvToMonster()
    {
        // @TODO missing implementation
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Monster()
    {
        // @TODO missing implementation
    }
    
    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Column()
    {
        // @TODO missing implementation
    }
}