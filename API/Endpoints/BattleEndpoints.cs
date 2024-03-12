using API.Endpoints.Internal;
using API.Extensions;
using API.Infrastructure;
using Application.Battles;
using Application.Battles.Create;
using Application.Battles.GetBattles;
using Application.Battles.GetById;
using Application.Battles.Remove;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MediatR;

namespace API.Endpoints
{
    public class BattleEndpoints() : IEndpoints
    {
        private const string ContentType = "application/json";
        private const string Tag = "Battles";
        private const string BaseRoute = "battles";
        private const string Slash = "/";

        public static void DefineEndpoints(IEndpointRouteBuilder app)
        {
            var battles = app.MapGroup(BaseRoute)
                .WithTags(Tag);

            battles.MapPost(Slash, CreateBattleAsync)
                .WithName("CreateBattle")
                .Accepts<CreateBattleRequest>(ContentType)
                .Produces<Guid>(201)
                .Produces<IEnumerable<ValidationFailure>>(400);

            battles.MapGet(Slash, GetAllBattlesAsync)
                .WithName("GetBattles")
                .Produces<BattleResponse[]>(200);

            battles.MapGet($"{Slash}{{id}}", GetBattleByIdAsync)
                .WithName("GetBattle")
                .Accepts<BattleResponse>(ContentType)
                .Produces<BattleResponse>(200).Produces(404);

            battles.MapDelete($"{Slash}{{id}}", DeleteBattleAsync)
                .WithName("DeleteBattle")
                .Produces(204)
                .Produces<IEnumerable<ValidationFailure>>(400);
        }

        internal static async Task<IResult> CreateBattleAsync
            (CreateBattleRequest request, ISender sender, CancellationToken cancellationToken)
        {
            var command = request.Adapt<CreateBattleCommand>();

            //var validationResult = await validator.ValidateAsync(battle);
            //if (!validationResult.IsValid)
            //{
            //    return Results.BadRequest(validationResult.Errors);
            //}

            var result = await sender.Send(command, cancellationToken);

            //if (!await repository.UnitOfWork.Commit())
            //{
            //    return Results.BadRequest(new List<ValidationFailure>
            //    {
            //        new("Id", "A battle with this Id already exists")
            //    });
            //}
            //Results.Created($"/{BaseRoute}/{result.Value}", result.Value)
            return result.Match(Results.Created, CustomResults.Problem);
        }
        internal static async Task<IResult> GetAllBattlesAsync(
            ISender sender, CancellationToken cancellationToken)
        {
            return Results.Ok(await sender.Send(new GetBattlesQuery(), cancellationToken));
        }
        internal static async Task<IResult> GetBattleByIdAsync(
            Guid battleId, ISender sender, CancellationToken cancellationToken)
        {
            var battle = await sender.Send(new GetBattleByIdQuery(battleId), cancellationToken);
            return battle is not null ? Results.Ok(battle) : Results.NotFound($"The battle with ID = {battleId} not found.");
        }
        internal static async Task<IResult> DeleteBattleAsync(
            Guid battleId, ISender sender, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new RemoveBattleCommand(battleId), cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        }
    }
}
