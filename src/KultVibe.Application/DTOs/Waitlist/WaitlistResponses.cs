namespace KultVibe.Application.DTOs.Waitlist;

/// <summary>201 payload for POST /api/waitlist: {id, email, message}.</summary>
public sealed class WaitlistSignupResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}

/// <summary>200 payload for GET /api/waitlist/count: {count}.</summary>
public sealed class WaitlistCountResponse
{
    public int Count { get; init; }
}

/// <summary>200 payload for GET /api/waitlist/exists/{email}: {exists}.</summary>
public sealed class WaitlistExistsResponse
{
    public bool Exists { get; init; }
}

/// <summary>A single row in the admin waitlist listing.</summary>
public sealed class WaitlistAdminEntryDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? GamerTag { get; init; }
    public string PrimaryVertical { get; init; } = string.Empty;
    public string? ReferralCode { get; init; }
    public bool ConfirmationEmailSent { get; init; }
    public DateTime CreatedAt { get; init; }
}
