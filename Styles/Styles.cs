using System.Collections.Immutable;

namespace HalloweenFx.MinApi.Styles;

public static class StylesMap
{
    public static readonly IReadOnlyDictionary<string, StyleTemplate> All =
        new Dictionary<string, StyleTemplate>(StringComparer.OrdinalIgnoreCase)
        {
            ["Zombie"] = new(
                "photorealistic zombie makeover on skin, hair and clothing; cinematic horror grade; identity preserved; KEEP FACIAL STRUCTURE (same facial geometry, proportions, jawline, eye spacing, nose and mouth shape); no reshaping",
                "cartoon, lowres, deformed, warped proportions, face morph, mutated features, extra teeth, enlarged mouth, asymmetry, out of frame",
                0.60
            ),
            ["Vampire"] = new(
                "elegant vampire on skin, hair glossy dark, clothing gothic; subtle fangs; identity preserved; KEEP FACIAL STRUCTURE (same bone structure and feature positions); no reshaping",
                "gore, cartoon, lowres, deformed, warped proportions, face morph, mutated features, asymmetry, out of frame",
                0.50
            ),
            ["Witch"] = new(
                "witch makeover with smoky eyes, neat rune paint, hat; dark clothing; moonlit grading; identity preserved; KEEP FACIAL STRUCTURE (no change to facial geometry or proportions)",
                "toyish, cartoon, lowres, messy paint, deformed, warped proportions, face morph, asymmetry",
                0.55
            ),
            ["PumpkinHead"] = new(
                "replace only the head surface with a carved jack-o’-lantern motif overlay while preserving underlying identity cues; body/clothes unchanged; spooky rim light; KEEP FACIAL STRUCTURE (no distortion to head shape/feature layout)",
                "melted face, warped body, deformed, warped proportions, face morph, cartoon, lowres",
                0.65
            ),
            ["Ghost"] = new(
                "ethereal semi-transparent glow across skin, hair, and clothing; cool blue grading; identity preserved; KEEP FACIAL STRUCTURE (no geometry changes)",
                "cartoon, lowres, heavy blur, deformed, warped proportions, face morph",
                0.45
            ),
            ["SkeletonMakeup"] = new(
                "day-of-the-dead skull face paint with clean, symmetric lines; subtle accents on hair/clothes; identity preserved; KEEP FACIAL STRUCTURE (no reshaping, same feature positions)",
                "smudged paint, cartoon, lowres, deformed, warped proportions, face morph, asymmetry",
                0.45
            ),
        }.ToImmutableDictionary();
}

public record StyleTemplate(string Prompt, string NegativePrompt, double Strength);
