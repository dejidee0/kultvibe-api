namespace KultVibe.Application.DTOs.Waitlist;

/// <summary>
/// Body for POST /api/waitlist. UserType defaults to Gamer when omitted.
/// PrimaryVertical is deprecated and optional for older clients.
/// </summary>
public sealed class WaitlistSignupRequest
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? GamerTag { get; init; }
    public string? UserType { get; init; }
    public string? PrimaryGame { get; init; }
    public string? CompanyName { get; init; }
    public string[]? Interests { get; init; }
    public string? PrimaryVertical { get; init; }
    public string? ReferralCode { get; init; }
}
