using HalloweenFx.MinApi.Options;

namespace HalloweenFx.MinApi.Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppOptions>(builder.Configuration);
        builder.Services.AddHttpClient();
        builder.Services.AddProblemDetails();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHealthChecks();
        builder.Services.AddResponseCaching();
        return builder;
    }
}