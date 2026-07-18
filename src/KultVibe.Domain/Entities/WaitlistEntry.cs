using KultVibe.Domain.Common;
using KultVibe.Domain.Enums;

namespace KultVibe.Domain.Entities;

/// <summary>
/// A single pre-launch waitlist signup. Phase 1's only persisted entity.
/// </summary>
public class WaitlistEntry : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? GamerTag { get; set; }
    public Vertical PrimaryVertical { get; set; }

    /// <summary>Referral code the signer used, if any (their referrer's code - not their own).</summary>
    public string? ReferralCode { get; set; }

    public bool ConfirmationEmailSent { get; set; }
}
