using System.Diagnostics;
using System.Threading;
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
using Microsoft.OpenApi.Any;
using Moq;

namespace API.Test;

public partial class MonsterEndpointsTests
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
        result.Should().BeOfType<Ok<Monster[]>>();
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
        result.Should().BeOfType<NotFound<string>>();
        var notFound = result as NotFound<string>;
        Assert.Equal($"The monster with ID = {id} not found.", notFound.Value);
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
        var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine($"{directory}/Files/", "monsters-correct.csv");

        MemoryStream ms = new();
        FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            ms.Write(bytes, 0, (int)fileStream.Length);
        }
        var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

        _repository
            .Setup(x => x.AddAsync(It.IsAny<IEnumerable<Monster>>()));

        _repository
            .Setup(x => x.UnitOfWork.Commit())
            .ReturnsAsync(true);

        var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _repository.Object);
        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Monster()
    {
        var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine($"{directory}/Files/", "monsters-empty-monster.csv");

        MemoryStream ms = new();
        FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            ms.Write(bytes, 0, (int)fileStream.Length);
        }
        var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

        _repository
            .Setup(x => x.AddAsync(It.IsAny<IEnumerable<Monster>>()));

        var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _repository.Object);
        result.Should().BeOfType<BadRequest<string>>();
        var badRequest = result as BadRequest<string>;
        Assert.Equal("Wrong data mapping.", badRequest.Value);
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Column()
    {
        var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine($"{directory}/Files/", "monsters-wrong-column.csv");

        MemoryStream ms = new();
        FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            ms.Write(bytes, 0, (int)fileStream.Length);
        }
        var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

        var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _repository.Object);
        result.Should().BeOfType<BadRequest<string>>();
        var badRequest = result as BadRequest<string>;
        Assert.Equal("Wrong data mapping.", badRequest.Value);
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Txt_File()
    {
        var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine($"{directory}/Files/", "Test.txt");

        MemoryStream ms = new();
        FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
        {
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            ms.Write(bytes, 0, (int)fileStream.Length);
        }
        var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

        var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _repository.Object);
        result.Should().BeOfType<BadRequest<string>>();
        var badRequest = result as BadRequest<string>;
        Assert.Equal("The extension is not supporting.", badRequest.Value);
    }
}