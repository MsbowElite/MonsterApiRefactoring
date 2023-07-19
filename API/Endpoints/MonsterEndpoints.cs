using API.Endpoints.Internal;
using FluentValidation;
using FluentValidation.Results;
using Lib.Repository.Entities;
using Lib.Repository.Repository;

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

            monsters.MapPut($"{Slash}{{isbn}}", UpdateMonsterAsync)
                .WithName("UpdateMonster")
                .Accepts<Monster>(ContentType)
                .Produces<Monster>(200).Produces<IEnumerable<ValidationFailure>>(400);

            monsters.MapDelete($"{Slash}{{isbn}}", DeleteMonsterAsync)
                .WithName("DeleteMonster")
                .Produces<Monster>(204).Produces<IEnumerable<ValidationFailure>>(400);
        }

        internal static async Task<IResult> CreateMonsterAsync(Monster monster, IMonsterRepository repository, IValidator<Monster> validator)
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
        internal static async Task<IResult> GetAllMonstersAsync(IMonsterRepository repository)
        {
            return Results.Ok(await repository.GetAllAsync());
        }
        internal static async Task<IResult> GetMonsterByIdAsync(int id, IMonsterRepository repository)
        {
            var monster = await repository.FindAsync(id);
            return monster is not null ? Results.Ok(monster) : Results.NotFound($"The monster with ID = {id} not found.");
        }
        internal static async Task<IResult> UpdateMonsterAsync(int id, Monster monster, IMonsterRepository repository, IValidator<Monster> validator)
        {
            monster.Id = id;
            var validationResult = await validator.ValidateAsync(monster);
            if (!validationResult.IsValid)
                return Results.BadRequest(validationResult.Errors);

            repository.Update(await repository.FindAsync(id), monster);
            return !await repository.UnitOfWork.Commit() ? Results.NotFound() : Results.Ok(monster);
        }
        internal static async Task<IResult> DeleteBookAsync(
            int id, IMonsterRepository repository)
        {
            var deleted = await repository.RemoveAsync(await repository.FindAsync(id));
            return await repository.UnitOfWork.Commit() ? Results.NoContent() : Results.NotFound();
        }
    }
}
