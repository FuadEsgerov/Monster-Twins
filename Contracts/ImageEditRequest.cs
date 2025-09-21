namespace HalloweenFx.MinApi.Contracts;

public class ImageEditRequest
{
    public string BaseImagePath { get; set; } = default!;
    public string? MaskImagePath { get; set; }
    public string Prompt { get; set; } = default!;
    public string? NegativePrompt { get; set; }
    public double? Strength { get; set; } 
    public string? AspectRatio { get; set; } 
}