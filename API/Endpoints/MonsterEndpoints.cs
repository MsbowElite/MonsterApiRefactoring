using API.Endpoints.Internal;
using API.Models;
using CsvHelper;
using FluentValidation;
using FluentValidation.Results;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;

namespace API.Endpoints
{
    public class MonsterEndpoints : IEndpoints
    {
        private const string ContentType = "application/json";
        private const string Tag = "Monsters";
        private const string BaseRoute = "monsters";
        private const string Slash = "/";

        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMonsterRepository, MonsterRepository>();
        }

        public static void DefineEndpoints(IEndpointRouteBuilder app)
        {
            var monsters = app.MapGroup(BaseRoute)
                .WithTags(Tag);

            monsters.MapPost(Slash, CreateMonsterAsync)
                .WithName("CreateMonster")
                .Accepts<Monster>(ContentType)
                .Produces<Monster>(201).Produces<IEnumerable<ValidationFailure>>(400);

            monsters.MapGet(Slash, GetAllMonstersAsync)
                .WithName("GetMonsters")
                .Produces<IEnumerable<Monster>>(200);

            monsters.MapGet($"{Slash}{{id}}", GetMonsterByIdAsync)
                .WithName("GetMonster")
                .Accepts<Monster>(ContentType)
                .Produces<Monster>(200).Produces(404);

            monsters.MapPut($"{Slash}{{id}}", UpdateMonsterAsync)
                .WithName("UpdateMonster")
                .Accepts<Monster>(ContentType)
                .Produces<Monster>(200).Produces<IEnumerable<ValidationFailure>>(400)
                .Produces<string>(422);

            monsters.MapDelete($"{Slash}{{id}}", DeleteMonsterAsync)
                .WithName("DeleteMonster")
                .Produces<Monster>(204).Produces<IEnumerable<ValidationFailure>>(400);

            monsters.MapPost($"{Slash}UploadCsvToImport", UploadCsvToImportAsync)
                .WithName("UploadCsvToImport")
                .Accepts<Monster>(ContentType)
                .Produces(204).Produces(400);
        }

        public static async Task<IResult> CreateMonsterAsync(Monster monster, IMonsterRepository repository, IValidator<Monster> validator)
        {
            var validationResult = await validator.ValidateAsync(monster);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            await repository.AddAsync(monster);

            if (!await repository.UnitOfWork.Commit())
            {
                return Results.BadRequest(new List<ValidationFailure>
                {
                    new("Id", "A monster with this Id already exists")
                });
            }

            return Results.Created($"/{BaseRoute}/{monster.Id}", monster);
        }
        public static async Task<IResult> GetAllMonstersAsync(IMonsterRepository repository)
        {
            return Results.Ok(await repository.GetAllAsync());
        }
        public static async Task<IResult> GetMonsterByIdAsync(int id, IMonsterRepository repository)
        {
            var monster = await repository.FindAsync(id);
            return monster is not null ? Results.Ok(monster) : Results.NotFound($"The monster with ID = {id} not found.");
        }
        public static async Task<IResult> UpdateMonsterAsync(int id, Monster monster, IMonsterRepository repository, IValidator<Monster> validator)
        {
            monster.Id = id;
            ValidationResult validationResult = await validator.ValidateAsync(monster);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            var foundMonster = await repository.FindAsync(id);
            if (foundMonster is null)
                return Results.NotFound($"The monster with ID = {id} not found.");

            return !await repository.UnitOfWork.Commit() ? Results.UnprocessableEntity(foundMonster) : Results.Ok(monster);
        }
        public static async Task<IResult> DeleteMonsterAsync(
            int id, IMonsterRepository repository)
        {
            repository.Remove(await repository.FindAsync(id));
            return await repository.UnitOfWork.Commit() ? Results.NoContent() : Results.NotFound($"The monster with ID = {id} not found.");
        }
        public static async Task<IResult> UploadCsvToImportAsync(IFormFile file, IMonsterRepository repository)
        {
            try
            {
                string ext = Path.GetExtension(file.FileName);
                string filename = Guid.NewGuid().ToString() + ext;
                string directory = Path.Combine(Directory.GetCurrentDirectory(), "Temp");
                string filepath = Path.Combine(directory, filename);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await using (FileStream fs = System.IO.File.Create(filepath))
                {
                    await file.CopyToAsync(fs);
                }

                if (ext != ".csv")
                {
                    return Results.BadRequest("The extension is not supporting.");
                }

                using (var reader = new StreamReader(filepath))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        try
                        {
                            var records = csv.GetRecords<MonsterToImport>().ToList();
                            var monsters = records.Select(x => new Monster()
                            {
                                Name = x.name,
                                Attack = x.attack,
                                Defense = x.defense,
                                Speed = x.speed,
                                Hp = x.hp,
                                ImageUrl = x.imageUrl
                            });

                            await repository.AddAsync(monsters);
                            await repository.UnitOfWork.Commit();

                            System.IO.File.Delete(filepath);
                            return Results.NoContent();
                        }
                        catch (Exception)
                        {
                            System.IO.File.Delete(filepath);
                            return Results.BadRequest("Wrong data mapping.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Results.BadRequest("Wrong data mapping.");
            }
        }
    }
}
