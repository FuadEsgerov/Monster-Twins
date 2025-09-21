namespace HalloweenFx.MinApi.Infrastructure.Extensions;

public static class CorsExtensions
{
    public static WebApplicationBuilder AddCorsDefaults(this WebApplicationBuilder builder)
    {
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Default", policy =>
            {
                if (origins.Length > 0)
                    policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
                else
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
        return builder;
    }
}