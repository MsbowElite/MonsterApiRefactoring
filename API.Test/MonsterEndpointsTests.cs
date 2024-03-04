using API.Endpoints;
using API.Test.Fixtures;
using Application.Abstractions.Data;
using Application.Monsters;
using Application.Monsters.GetById;
using Application.Monsters.GetMonsters;
using Domain.Monsters;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SharedKernel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Test;

public partial class MonsterEndpointsTests
{
    private readonly Mock<ISender> _sender;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public MonsterEndpointsTests()
    {
        _sender = new Mock<ISender>();
        _unitOfWork = new Mock<IUnitOfWork>();
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfMonsters()
    {
        Result<MonsterResponse[]> monstersResult = Result.Success(MonsterFixture.GetMonstersResponseMock().ToArray());

        _sender
            .Setup(s => s.Send(new GetMonsersQuery(), default))
            .ReturnsAsync(monstersResult);

        IResult result = await MonsterEndpoints.GetMonstersAsync(_sender.Object, default);
        result.Should().BeOfType<Ok<MonsterResponse[]>>();
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsOneMonsterById()
    {
        MonsterResponse[] monsters = MonsterFixture.GetMonstersResponseMock().ToArray();

        MonsterResponse monster = monsters[0];
        _sender
            .Setup(s => s.Send(new GetMonserByIdQuery(monster.Id), default))
            .ReturnsAsync(monster);

        IResult result = await MonsterEndpoints.GetMonsterByIdAsync(monster.Id, _sender.Object, default);
        result.Should().BeOfType<Ok<MonsterResponse>>();
    }

    [Fact]
    public async Task Get_OnNoMonsterFound_Returns404()
    {
        var id = Guid.NewGuid();

        _sender
            .Setup(x => x.Send(new GetMonserByIdQuery(id), default))
            .ReturnsAsync(Result.Failure<MonsterResponse>(MonsterErrors.NotFound(id)));

        IResult result = await MonsterEndpoints.GetMonsterByIdAsync(id, _sender.Object, default);
        result.Should().BeOfType<NotFound<MonsterResponse>>();
    }

    //[Fact]
    //public async Task Delete_OnSuccess_RemoveMonster()
    //{
    //    const int id = 1;
    //    Monster[] monsters = MonsterFixture.GetMonstersMock().ToArray();

    //    _sender
    //        .Setup(x => x.FindAsync(id))
    //        .ReturnsAsync(() => monsters[0]);

    //    _sender
    //       .Setup(x => x.Remove(monsters[0]));

    //    _sender
    //        .Setup(x => x.UnitOfWork.Commit())
    //        .ReturnsAsync(true);

    //    IResult result = await MonsterEndpoints.DeleteMonsterAsync(id, _sender.Object);
    //    result.Should().BeOfType<NoContent>();
    //}

    //[Fact]
    //public async Task Delete_OnNoMonsterFound_Returns404()
    //{
    //    const int id = 123;

    //    _sender
    //       .Setup(x => x.Remove(null));

    //    _sender
    //        .Setup(x => x.FindAsync(id))
    //        .ReturnsAsync(() => null);

    //    _sender
    //        .Setup(x => x.UnitOfWork.Commit())
    //        .ReturnsAsync(false);

    //    var result = await MonsterEndpoints.DeleteMonsterAsync(id, _sender.Object);
    //    result.Should().BeOfType<NotFound<string>>();
    //    var notFound = result as NotFound<string>;
    //    Assert.Equal($"The monster with ID = {id} not found.", notFound.Value);
    //}

    //[Fact]
    //public async Task Post_OnSuccess_ImportCsvToMonster()
    //{
    //    var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    //    var path = Path.Combine($"{directory}/Files/", "monsters-correct.csv");

    //    MemoryStream ms = new();
    //    FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
    //    {
    //        byte[] bytes = new byte[fileStream.Length];
    //        fileStream.Read(bytes, 0, (int)fileStream.Length);
    //        ms.Write(bytes, 0, (int)fileStream.Length);
    //    }
    //    var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

    //    _sender
    //        .Setup(x => x.AddAsync(It.IsAny<IEnumerable<Monster>>()));

    //    _sender
    //        .Setup(x => x.UnitOfWork.Commit())
    //        .ReturnsAsync(true);

    //    var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _sender.Object);
    //    result.Should().BeOfType<NoContent>();
    //}

    //[Fact]
    //public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Monster()
    //{
    //    var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    //    var path = Path.Combine($"{directory}/Files/", "monsters-empty-monster.csv");

    //    MemoryStream ms = new();
    //    FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
    //    {
    //        byte[] bytes = new byte[fileStream.Length];
    //        fileStream.Read(bytes, 0, (int)fileStream.Length);
    //        ms.Write(bytes, 0, (int)fileStream.Length);
    //    }
    //    var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

    //    _sender
    //        .Setup(x => x.AddAsync(It.IsAny<IEnumerable<Monster>>()));

    //    var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _sender.Object);
    //    result.Should().BeOfType<BadRequest<string>>();
    //    var badRequest = result as BadRequest<string>;
    //    Assert.Equal("Wrong data mapping.", badRequest.Value);
    //}

    //[Fact]
    //public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Column()
    //{
    //    var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    //    var path = Path.Combine($"{directory}/Files/", "monsters-wrong-column.csv");

    //    MemoryStream ms = new();
    //    FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
    //    {
    //        byte[] bytes = new byte[fileStream.Length];
    //        fileStream.Read(bytes, 0, (int)fileStream.Length);
    //        ms.Write(bytes, 0, (int)fileStream.Length);
    //    }
    //    var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

    //    var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _sender.Object);
    //    result.Should().BeOfType<BadRequest<string>>();
    //    var badRequest = result as BadRequest<string>;
    //    Assert.Equal("Wrong data mapping.", badRequest.Value);
    //}

    //[Fact]
    //public async Task Post_BadRequest_ImportCsv_With_Txt_File()
    //{
    //    var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    //    var path = Path.Combine($"{directory}/Files/", "Test.txt");

    //    MemoryStream ms = new();
    //    FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
    //    {
    //        byte[] bytes = new byte[fileStream.Length];
    //        fileStream.Read(bytes, 0, (int)fileStream.Length);
    //        ms.Write(bytes, 0, (int)fileStream.Length);
    //    }
    //    var file = new FormFile(fileStream, 0, fileStream.Length, "name", fileStream.Name);

    //    var result = await MonsterEndpoints.UploadCsvToImportAsync(file, _sender.Object);
    //    result.Should().BeOfType<BadRequest<string>>();
    //    var badRequest = result as BadRequest<string>;
    //    Assert.Equal("The extension is not supporting.", badRequest.Value);
    //}
}