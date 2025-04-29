using BattleShip.Common.Enums;

namespace BattleShip.API.Endpoints.Field;

public static class FieldEndpointRegistration
{
    public static void RegisterFieldEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/gameplay/field/generate", GenerateFieldEndpoint.ExecuteAsync)
            .WithName("Generate Field")
            .WithTags("Gameplay")
            .Produces<int[,]>()
            .WithOpenApi();
    }
}
