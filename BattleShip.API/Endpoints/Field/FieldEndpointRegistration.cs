using BattleShip.Application.Models;
using BattleShip.Common.Enums;

namespace BattleShip.API.Endpoints.Field;

public static class FieldEndpointRegistration
{
    public static void RegisterFieldEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/gameplay/field/generate", GenerateFieldEndpoint.ExecuteAsync)
            .WithName("Generate Field")
            .WithTags("Gameplay")
            .Produces<GameFieldDto>()
            .WithOpenApi();

        app.MapPost("/gameplay/field/check-cell", CheckCellEndpoint.ExecuteAsync)
            .WithName("Check cell")
            .WithTags("Gameplay")
            .Produces<CheckCellResponse>()
            .WithOpenApi();

        app.MapPut("/gameplay/field/regenerate", RegenerateFieldEndpoint.ExecuteAsync)
            .WithName("Regenerate Field")
            .WithTags("Gameplay")
            .Produces<GameFieldDto>()
            .WithOpenApi();

        app.MapGet("/gameplay/field", GetGameFieldEndpoint.ExecuteAsync)
            .WithName("Get Game Field")
            .WithTags("Gameplay")
            .Produces<GameFieldDto>()
            .WithOpenApi();

        app.MapGet("/gameplay/field/computer-move", GetComputerCheckCellEndpoint.ExecuteAsync)
            .WithName("Get Field after computer move")
            .WithTags("Gameplay")
            .Produces<CheckCellResponse>()
            .WithOpenApi();
    }
}
