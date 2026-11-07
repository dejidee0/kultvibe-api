namespace KultVibe.Domain.Common;

public static class WaitlistInterests
{
    public static IReadOnlyList<string> AllowedValues { get; } =
        Array.AsReadOnly(new[] { "Music", "Tech", "Lifestyle" });
}
