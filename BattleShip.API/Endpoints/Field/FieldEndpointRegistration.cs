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

        app.MapPut("/gameplay/field/regenerate", RegenerateFieldEndpoint.ExecuteAsync)
            .WithName("Regenerate Field")
            .WithTags("Gameplay")
            .Produces<GameFieldDto>()
            .WithOpenApi();
    }
}
