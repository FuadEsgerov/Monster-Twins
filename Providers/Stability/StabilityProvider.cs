using System.Net.Http.Headers;
using HalloweenFx.MinApi.Contracts;

namespace HalloweenFx.MinApi.Providers.Stability;

public sealed class StabilityProvider(HttpClient http, string apiBase, string apiKey, string _ignored)
    : IImageEditProvider
{
    public async Task<byte[]> EditAsync(ImageEditRequest req)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("STABILITY_API_KEY missing");

        var url = $"{apiBase.TrimEnd('/')}/v2alpha/generation/stable-image/inpaint";
        using var form = new MultipartFormDataContent();

        static void SetCdName(HttpContent c, string name, string? fileName = null)
        {
            var cd = new ContentDispositionHeaderValue("form-data") { Name = $"\"{name}\"" };
            if (!string.IsNullOrEmpty(fileName)) cd.FileName = $"\"{fileName}\"";
            c.Headers.ContentDisposition = cd;
        }

        var promptText = string.IsNullOrWhiteSpace(req.Prompt)
            ? "photorealistic portrait of the same person, identity preserved, detailed zombie makeover: pale gray skin, sunken dark eyes, cracked lips, dried blood stains, subtle scars, slightly rotting texture, cinematic horror look, 35mm lens"
            : req.Prompt;
        var prompt = new StringContent(promptText);
        SetCdName(prompt, "prompt");
        form.Add(prompt);

        var fmt = new StringContent("png");
        SetCdName(fmt, "output_format");
        form.Add(fmt);

        if (!string.IsNullOrWhiteSpace(req.NegativePrompt))
        {
            var neg = new StringContent(req.NegativePrompt!);
            SetCdName(neg, "negative_prompt");
            form.Add(neg);
        }

        var mode = new StringContent("search");
        SetCdName(mode, "mode");
        form.Add(mode);

        var search = new StringContent("face");
        SetCdName(search, "search_prompt");
        form.Add(search);

        if (string.IsNullOrWhiteSpace(req.BaseImagePath))
            throw new InvalidOperationException("BaseImagePath is required for Stability inpaint");
        var inName = Path.GetFileName(req.BaseImagePath);
        var inMime = GetMimeFromExtension(inName);
        var img = new StreamContent(File.OpenRead(req.BaseImagePath));
        img.Headers.ContentType = new MediaTypeHeaderValue(inMime);
        SetCdName(img, "image", inName);
        form.Add(img);


        using var msg = new HttpRequestMessage(HttpMethod.Post, url) { Content = form };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        msg.Headers.Accept.Clear();
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("image/*")); // return raw image bytes :contentReference[oaicite:2]{index=2}

        using var resp = await http.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead);
        if (!resp.IsSuccessStatusCode)
        {
            var errBody = await resp.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Stability error {(int)resp.StatusCode}: {errBody}");
        }

        return await resp.Content.ReadAsByteArrayAsync();
    }

    private static string GetMimeFromExtension(string fileName)
    {
        return Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".png"  => "image/png",
            ".jpg"  => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".webp" => "image/webp",
            _       => "application/octet-stream"
        };
    }
}
