namespace HalloweenFx.MinApi.Infrastructure.Extensions;

public sealed class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext ctx)
    {
        var h = ctx.Response.Headers;
        h["X-Content-Type-Options"] = "nosniff";
        h["X-Frame-Options"] = "DENY";
        h["Referrer-Policy"] = "no-referrer";
        h["Cross-Origin-Resource-Policy"] = "cross-origin";
        await next(ctx);
    }
}