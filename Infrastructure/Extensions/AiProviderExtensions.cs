using HalloweenFx.MinApi.Options;
using HalloweenFx.MinApi.Providers;
using HalloweenFx.MinApi.Providers.Stability;
using Microsoft.Extensions.Options;

namespace HalloweenFx.MinApi.Infrastructure.Extensions;

public static class AiProviderExtensions
{
    public static WebApplicationBuilder AddAiProviders(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IImageEditProvider>(sp =>
        {
            var cfg  = sp.GetRequiredService<IOptions<AppOptions>>().Value;
            var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
            var prov = (cfg.Ai.Provider ?? "Stability").Trim().ToLowerInvariant();

            return prov switch
            {
                "stability" => new StabilityProvider(
                    http,
                    cfg.Ai.Stability.ApiBase ?? "https://api.stability.ai",
                    Environment.GetEnvironmentVariable("STABILITY_API_KEY") ?? "",
                    cfg.Ai.Stability.Engine ?? "sd3.5-medium"
                ),
                _ => throw new InvalidOperationException(
                    $"Unknown AI provider '{prov}'. Expected: stability.")
            };
        });
        return builder;
    }
}