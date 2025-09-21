using HalloweenFx.MinApi.Styles;

namespace HalloweenFx.MinApi.Infrastructure.Endpoints;

public static class StylesEndpoints
{
    public static void MapStylesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => Results.Ok(new { ok = true, message = "🎃 Halloween FX Minimal API" }));
        app.MapGet("/api/styles", () => StylesMap.All.Keys);
    }
}