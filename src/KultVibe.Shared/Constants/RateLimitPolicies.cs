namespace KultVibe.Shared.Constants;

public static class RateLimitPolicies
{
    /// <summary>Applied to POST /api/waitlist. Master Plan 2.2: 429 after repeated signup attempts.</summary>
    public const string WaitlistSignup = "waitlist-signup";
}
