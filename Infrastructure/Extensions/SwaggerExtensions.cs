namespace HalloweenFx.MinApi.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerDefaults(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
    }
}