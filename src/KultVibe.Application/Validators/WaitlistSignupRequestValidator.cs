using FluentValidation;
using KultVibe.Application.DTOs.Waitlist;
using KultVibe.Domain.Common;
using KultVibe.Domain.Enums;

namespace KultVibe.Application.Validators;

/// <summary>
/// Validation messages here are pinned to the Master Plan v2.0 Phase 1 Error Cases table -
/// do not reword them without checking the frontend isn't matching on the exact string.
/// </summary>
public sealed class WaitlistSignupRequestValidator : AbstractValidator<WaitlistSignupRequest>
{
    private static readonly string[] ValidVerticals = Enum.GetNames<Vertical>();
    private static readonly string[] ValidUserTypes = Enum.GetNames<UserType>();

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
            .Must(v => ValidVerticals.Contains(v?.Trim(), StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Primary vertical must be one of: {string.Join(", ", ValidVerticals)}")
            .When(x => x.PrimaryVertical is not null);

        RuleFor(x => x.UserType)
            .Must(value => ValidUserTypes.Contains(value?.Trim(), StringComparer.OrdinalIgnoreCase))
            .WithMessage("User type must be one of: Gamer, Creator, Brand")
            .When(x => x.UserType is not null);

        RuleFor(x => x.PrimaryGame)
            .MaximumLength(100);

        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required for brands")
            .When(x => string.Equals(x.UserType?.Trim(), nameof(UserType.Brand), StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.CompanyName)
            .MaximumLength(150);

        RuleFor(x => x.Interests)
            .Must(values => values is null || (values.Length <= 3 && values.All(value =>
                WaitlistInterests.AllowedValues.Contains(value?.Trim(), StringComparer.OrdinalIgnoreCase))))
            .WithMessage("Interests must be any of: Music, Tech, Lifestyle");

        RuleFor(x => x.GamerTag)
            .MaximumLength(50);

        RuleFor(x => x.ReferralCode)
            .MaximumLength(20);
    }
}
