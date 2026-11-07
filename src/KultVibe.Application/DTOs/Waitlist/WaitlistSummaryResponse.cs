using KultVibe.Domain.Enums;

namespace KultVibe.Application.DTOs.Waitlist;

public sealed class WaitlistSummaryResponse
{
    public int Total { get; init; }
    public Dictionary<string, int> ByUserType { get; init; } = new();
    public Dictionary<string, int> ByInterest { get; init; } = new();
    public List<WaitlistGameCountDto> TopGames { get; init; } = new();
    public Dictionary<string, int> ByLegacyVertical { get; init; } = new();
}

public sealed class WaitlistGameCountDto
{
    public string Game { get; init; } = string.Empty;
    public int Count { get; init; }
}

/// <summary>Only the fields needed to aggregate the waitlist; excludes personal identifiers.</summary>
public sealed class WaitlistSummaryRow
{
    public UserType UserType { get; init; }
    public string? PrimaryGame { get; init; }
    public string? Interests { get; init; }
    public Vertical? PrimaryVertical { get; init; }
}
