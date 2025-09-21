using HalloweenFx.MinApi.Infrastructure.Endpoints;
using HalloweenFx.MinApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddKestrelDefaults()
    .AddCoreServices()
    .AddCorsDefaults()
    .AddAiProviders()
    .AddRateLimitingDefaults()
    .AddSwaggerDefaults();

var app = builder.Build();

app.UseExceptionHandler();
app.UseRateLimiter();
app.UseCors("Default");
app.UseResponseCaching();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthEndpoints();
app.MapStylesEndpoints();
app.MapImageEndpoints();

app.Run();