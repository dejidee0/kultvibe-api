namespace KultVibe.Application.DTOs.Waitlist;

/// <summary>
/// Body for POST /api/waitlist. Master Plan v2.0, Phase 1 API Endpoints table.
/// PrimaryVertical arrives as a string (e.g. "Gaming") and is validated/parsed
/// against the Vertical enum by WaitlistSignupRequestValidator.
/// </summary>
public sealed class WaitlistSignupRequest
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? GamerTag { get; init; }
    public string PrimaryVertical { get; init; } = string.Empty;
    public string? ReferralCode { get; init; }
}
