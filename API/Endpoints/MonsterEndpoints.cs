using API.Endpoints.Internal;
using API.Extensions;
using API.Infrastructure;
using Application.Monsters;
using Application.Monsters.Create;
using Application.Monsters.GetById;
using Application.Monsters.GetMonsters;
using Application.Monsters.Remove;
using Application.Monsters.Update;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MediatR;

namespace API.Endpoints
{
    public class MonsterEndpoints() : IEndpoints
    {
        private const string ContentType = "application/json";
        private const string Tag = "Monsters";
        private const string BaseRoute = "monsters";
        private const string Slash = "/";

        public static void DefineEndpoints(IEndpointRouteBuilder app)
        {
            var monsters = app.MapGroup(BaseRoute)
                .WithTags(Tag);

            monsters.MapPost(Slash, CreateMonsterAsync)
                .WithName("CreateMonster")
                .Accepts<CreateMonsterRequest>(ContentType)
                .Produces<Guid>(201)
                .Produces<IEnumerable<ValidationFailure>>(400);

            monsters.MapGet(Slash, GetMonstersAsync)
                .WithName("GetMonsters")
                .Produces<IEnumerable<MonsterResponse>>(200);

            monsters.MapGet($"{Slash}{{id}}", GetMonsterByIdAsync)
                .WithName("GetMonster")
                .Produces<MonsterResponse>(200)
                .Produces<string>(404);

            monsters.MapPut($"{Slash}{{id}}", UpdateMonsterAsync)
                .WithName("UpdateMonster")
                .Accepts<UpdateMonsterRequest>(ContentType)
                .Produces(204)
                .Produces<IEnumerable<ValidationFailure>>(400)
                .Produces<string>(404)
                .Produces(422);

            monsters.MapDelete($"{Slash}{{id}}", DeleteMonsterAsync)
                .WithName("DeleteMonster")
                .Produces<string>(404)
                .Produces(204);

            //monsters.MapPost($"{Slash}UploadCsvToImport", UploadCsvToImportAsync)
            //    .WithName("UploadCsvToImport")
            //    .Accepts<Monster>(ContentType)
            //    .Produces(204)
            //    .Produces<string>(400);
        }

        public static async Task<IResult> CreateMonsterAsync(
            CreateMonsterRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            //var validationResult = await validator.ValidateAsync(monster);
            //if (!validationResult.IsValid)
            //{
            //    return Results.BadRequest(validationResult.Errors);
            //}

            //await repository.AddAsync(monster);

            //if (!await repository.UnitOfWork.Commit())
            //{
            //    return Results.BadRequest(new List<ValidationFailure>
            //    {
            //        new("Id", "A monster with this Id already exists")
            //    });
            //}
            var command = request.Adapt<CreateMonsterCommand>();
            var result = await sender.Send(command, cancellationToken);

            return Results.Created($"/{BaseRoute}/{result.Value}", result.Value);
        }

        public static async Task<IResult> GetMonstersAsync(
            ISender sender, CancellationToken cancellationToken)
        {
            return Results.Ok((await sender.Send(new GetMonsersQuery(), cancellationToken)).Value);
        }
        public static async Task<IResult> GetMonsterByIdAsync(Guid MonsterId,
            ISender sender, CancellationToken cancellationToken)
        {
            var monster = (await sender.Send(new GetMonserByIdQuery(MonsterId), cancellationToken)).Value;
            return monster is not null ? Results.Ok(monster) : Results.NotFound($"The monster with ID = {MonsterId} not found.");
        }
        public static async Task<IResult> UpdateMonsterAsync(Guid MonsterId, UpdateMonsterRequest monster,
            ISender sender, CancellationToken cancellationToken)
        {
            //ValidationResult validationResult = await validator.ValidateAsync(monster);
            //if (!validationResult.IsValid)
            //    return Results.BadRequest(validationResult.Errors);

            var command = monster.Adapt<UpdateMonsterCommand>();
            var result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        }
        public static async Task<IResult> DeleteMonsterAsync(Guid monsterId,
            ISender sender, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new RemoveMonsterCommand(monsterId), cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        }
        //public static async Task<IResult> UploadCsvToImportAsync(IFormFile file, IMonsterRepository repository)
        //{
        //    try
        //    {
        //        string ext = Path.GetExtension(file.FileName);
        //        string filename = Guid.NewGuid().ToString() + ext;
        //        string directory = Path.Combine(Directory.GetCurrentDirectory(), "Temp");
        //        string filepath = Path.Combine(directory, filename);

        //        if (!Directory.Exists(directory))
        //        {
        //            Directory.CreateDirectory(directory);
        //        }

        //        await using (FileStream fs = System.IO.File.Create(filepath))
        //        {
        //            await file.CopyToAsync(fs);
        //        }

        //        if (ext != ".csv")
        //        {
        //            return Results.BadRequest("The extension is not supporting.");
        //        }
        //        try
        //        {
        //            using (var reader = new StreamReader(filepath))
        //            {
        //                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //                {
        //                    var records = csv.GetRecords<MonsterToImport>().ToList();

        //                    var monsters = records.Select(x => new Monster()
        //                    {
        //                        Name = x.name,
        //                        Attack = x.attack,
        //                        Defense = x.defense,
        //                        Speed = x.speed,
        //                        Hp = x.hp,
        //                        ImageUrl = x.imageUrl
        //                    });

        //                    await repository.AddAsync(monsters);
        //                    await repository.UnitOfWork.Commit();
        //                }
        //            }

        //            File.Delete(filepath);
        //            return Results.NoContent();
        //        }
        //        catch (Exception)
        //        {
        //            File.Delete(filepath);
        //            return Results.BadRequest("Wrong data mapping.");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Results.BadRequest("Wrong data mapping.");
        //    }
        //}
    }
}
