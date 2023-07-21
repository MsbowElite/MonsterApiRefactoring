using API.Endpoints.Internal;
using API.Models;
using CsvHelper;
using FluentValidation.Results;
using FluentValidation;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using System.Globalization;

namespace API.Endpoints
{
    public class BattleEndpoints : IEndpoints
    {
        private const string ContentType = "application/json";
        private const string Tag = "Battles";
        private const string BaseRoute = "battles";
        private const string Slash = "/";

        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBattleRepository, BattleRepository>();
        }

        public static void DefineEndpoints(IEndpointRouteBuilder app)
        {
            var battles = app.MapGroup(BaseRoute)
                .WithTags(Tag);

            battles.MapPost(Slash, CreateBattleAsync)
                .WithName("CreateBattle")
                .Accepts<Battle>(ContentType)
                .Produces<Battle>(201).Produces<IEnumerable<ValidationFailure>>(400);

            battles.MapGet(Slash, GetAllBattlesAsync)
                .WithName("GetBattles")
                .Produces<IEnumerable<Battle>>(200);

            battles.MapGet($"{Slash}{{id}}", GetBattleByIdAsync)
                .WithName("GetBattle")
                .Accepts<Battle>(ContentType)
                .Produces<Battle>(200).Produces(404);

            battles.MapDelete($"{Slash}{{id}}", DeleteBattleAsync)
                .WithName("DeleteBattle")
                .Produces<Battle>(204).Produces<IEnumerable<ValidationFailure>>(400);
        }

        internal static async Task<IResult> CreateBattleAsync(Battle battle, IBattleRepository repository, IValidator<Battle> validator)
        {
            var validationResult = await validator.ValidateAsync(battle);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            await repository.AddAsync(battle);

            if (!await repository.UnitOfWork.Commit())
            {
                return Results.BadRequest(new List<ValidationFailure>
                {
                    new("Id", "A battle with this Id already exists")
                });
            }

            return Results.Created($"/{BaseRoute}/{battle.Id}", battle);
        }
        internal static async Task<IResult> GetAllBattlesAsync(IBattleRepository repository)
        {
            return Results.Ok(await repository.GetAllAsync());
        }
        internal static async Task<IResult> GetBattleByIdAsync(int id, IBattleRepository repository)
        {
            var battle = await repository.FindAsync(id);
            return battle is not null ? Results.Ok(battle) : Results.NotFound($"The battle with ID = {id} not found.");
        }
        internal static async Task<IResult> DeleteBattleAsync(
            int id, IBattleRepository repository)
        {
            var deleted = await repository.RemoveAsync(await repository.FindAsync(id));
            return await repository.UnitOfWork.Commit() ? Results.NoContent() : Results.NotFound();
        }
    }
}
