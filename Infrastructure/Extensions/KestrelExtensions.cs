namespace HalloweenFx.MinApi.Infrastructure.Extensions;

public static class KestrelExtensions
{
    public static WebApplicationBuilder AddKestrelDefaults(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(k =>
        {
            k.Limits.MaxRequestBodySize = 20 * 1024 * 1024; // 20 MB
        });
        return builder;
    }
}