using System.Net.Mime;
using HalloweenFx.MinApi.Contracts;
using HalloweenFx.MinApi.Providers;
using HalloweenFx.MinApi.Styles;

namespace HalloweenFx.MinApi.Infrastructure.Endpoints;

public static class ImageEndpoints
{
    public static void MapImageEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/edit", async (HttpRequest request, IImageEditProvider ai) =>
        {
            try
            {
                if (!request.HasFormContentType)
                    return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);

                var form = await request.ReadFormAsync();

                var style = form["style"].ToString();
                if (string.IsNullOrWhiteSpace(style) || !StylesMap.All.TryGetValue(style, out var template))
                    return Results.BadRequest(new { error = "Unknown style. Try GET /api/styles" });

                var file = form.Files["image"];
                if (file is null || file.Length == 0)
                    return Results.BadRequest(new { error = "image file is required" });

                var allowedExt = new[] { ".png", ".jpg", ".jpeg", ".webp" };
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExt.Contains(ext) || !file.ContentType.StartsWith("image/"))
                    return Results.BadRequest(new { error = "Only PNG/JPG/WEBP images are supported." });

                if (file.Length > 20 * 1024 * 1024)
                    return Results.BadRequest(new { error = "Image too large (max 20MB)." });

                var id = Guid.NewGuid().ToString("n");
                var root = request.HttpContext.RequestServices
                    .GetRequiredService<IConfiguration>()["Storage:Root"] ?? "storage";
                Directory.CreateDirectory(root);

                var origPath = Path.Combine(root, $"{id}_orig{ext}");
                await using (var fs = File.Create(origPath)) await file.CopyToAsync(fs);

                string? maskPath = null;
                if (form.Files["mask"] is { Length: > 0 } mask)
                {
                    var mExt = Path.GetExtension(mask.FileName).ToLowerInvariant();
                    if (!allowedExt.Contains(mExt) || !mask.ContentType.StartsWith("image/"))
                        return Results.BadRequest(new { error = "Mask must be an image (PNG/JPG/WEBP)." });

                    maskPath = Path.Combine(root, $"{id}_mask{mExt}");
                    await using var ms = File.Create(maskPath);
                    await mask.CopyToAsync(ms);
                }

                var extra = form["prompt"].ToString();
                var prompt = string.IsNullOrWhiteSpace(extra) ? template.Prompt : $"{template.Prompt}. {extra}";

                var bytes = await ai.EditAsync(new ImageEditRequest
                {
                    BaseImagePath   = origPath,
                    MaskImagePath   = maskPath,
                    Prompt          = template.Prompt,
                    NegativePrompt  = template.NegativePrompt,
                    Strength        = template.Strength,
                    AspectRatio     = "1:1"
                });

                var outPath = Path.Combine(root, $"{id}_out.png");
                await File.WriteAllBytesAsync(outPath, bytes);

                return Results.Ok(new { id, style, url = $"/api/images/{id}" });
            }
            catch (HttpRequestException ex)
            {
                return Results.Problem(title: "Upstream AI error", detail: ex.Message, statusCode: 502);
            }
            catch (Exception ex)
            {
                return Results.Problem(title: "Image processing failed", detail: ex.Message, statusCode: 500);
            }
        })
        .Accepts<IFormFile>("multipart/form-data")
        .RequireRateLimiting("edit-tight");

        app.MapGet("/api/images/{id}", (string id, HttpContext ctx) =>
        {
            var root = ctx.RequestServices.GetRequiredService<IConfiguration>()["Storage:Root"] ?? "storage";
            var path = Path.Combine(root, $"{id}_out.png");
            if (!File.Exists(path)) return Results.NotFound();
            ctx.Response.Headers.CacheControl = "public, max-age=31536000, immutable";
            return Results.File(path, MediaTypeNames.Image.Png, enableRangeProcessing: true);
        });
    }
}