namespace BattleShip.API.Constraints;

public class StringConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext httpContext,
        IRouter route,
        string routeKey,
        RouteValueDictionary values,
        RouteDirection routeDirection)
    {
        return values[routeKey] is string;
    }
}
