namespace HalloweenFx.MinApi.Options;

public record AppOptions
{
    public StorageOptions Storage { get; init; } = new();
    public AiOptions Ai { get; init; } = new();
}

public record StorageOptions
{
    public string Root { get; init; } = "storage";
}

public record AiOptions
{
    public string Provider { get; init; } = "Stability";
    public StabilityOptions Stability { get; init; } = new();
}

public record StabilityOptions
{
    public string? ApiBase { get; init; } = "https://api.stability.ai";
    public string? Engine  { get; init; } = "sd3.5-medium";
}

