using BattleShip.Application.Models;

namespace BattleShip.API.Endpoints.Session;

public static class SessionEndpointRegistration
{
    public static void RegisterSessionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/gameplay/session/create", StartNewSessionEndpoint.ExecuteAsync)
            .WithName("Create Game Session")
            .WithTags("Session")
            .Produces<SessionDto>()
            .WithOpenApi();

        app.MapPost("/gameplay/session/{sessionId:string}/start-play", StartPlayGameEndpoint.ExecuteAsync)
            .WithName("Start play game in this session")
            .WithTags("Session")
            .Produces<SessionDto>()
            .WithOpenApi();

        app.MapGet("/gameplay/session/in-progress", GetSessionInProgressEndpoint.ExecuteAsync)
            .WithName("Check InProgress Game Session")
            .WithTags("Session")
            .Produces<string?>()
            .WithOpenApi();
    }
}
