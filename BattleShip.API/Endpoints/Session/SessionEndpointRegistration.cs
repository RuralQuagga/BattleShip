namespace BattleShip.API.Endpoints.Session;

public static class SessionEndpointRegistration
{
    public static void RegisterSessionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/gameplay/session/create", StartNewSessionEndpoint.ExecuteAsync)
            .WithName("Create Game Session")
            .WithTags("Session")
            .Produces<string>()
            .WithOpenApi();

        app.MapPost("/gameplay/session/{sessionId:string}/start-play", StartPlayGameEndpoint.ExecuteAsync)
            .WithName("Start play game in this session")
            .WithTags("Session")
            .Produces<string>()
            .WithOpenApi();
    }
}
