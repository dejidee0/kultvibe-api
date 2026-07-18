using FluentValidation;
using KultVibe.Application.DTOs.Waitlist;
using KultVibe.Domain.Enums;

namespace KultVibe.Application.Validators;

/// <summary>
/// Validation messages here are pinned to the Master Plan v2.0 Phase 1 Error Cases table -
/// do not reword them without checking the frontend isn't matching on the exact string.
/// </summary>
public sealed class WaitlistSignupRequestValidator : AbstractValidator<WaitlistSignupRequest>
{
    private static readonly string[] ValidVerticals = Enum.GetNames<Vertical>();

    public WaitlistSignupRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName is required")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Please enter a valid email address")
            .MaximumLength(255);

        RuleFor(x => x.PrimaryVertical)
            .NotEmpty().WithMessage("PrimaryVertical is required")
            .Must(v => ValidVerticals.Contains(v, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Primary vertical must be one of: {string.Join(", ", ValidVerticals)}");

        RuleFor(x => x.GamerTag)
            .MaximumLength(50);

        RuleFor(x => x.ReferralCode)
            .MaximumLength(20);
    }
}
