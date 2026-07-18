using FluentValidation;
using KultVibe.Application.DTOs.Waitlist;
using KultVibe.Application.Interfaces;
using KultVibe.Domain.Entities;
using KultVibe.Domain.Enums;
using KultVibe.Domain.Exceptions;
using KultVibe.Shared.Wrappers;
using Microsoft.Extensions.Logging;
using ValidationException = KultVibe.Domain.Exceptions.ValidationException;

namespace KultVibe.Application.Services;

public sealed class WaitlistService : IWaitlistService
{
    private readonly IWaitlistRepository _repository;
    private readonly IEmailService _emailService;
    private readonly IValidator<WaitlistSignupRequest> _validator;
    private readonly ILogger<WaitlistService> _logger;

    public WaitlistService(
        IWaitlistRepository repository,
        IEmailService emailService,
        IValidator<WaitlistSignupRequest> validator,
        ILogger<WaitlistService> logger)
    {
        _repository = repository;
        _emailService = emailService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<WaitlistSignupResponse> SignupAsync(WaitlistSignupRequest request, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        if (await _repository.ExistsByEmailAsync(normalizedEmail, ct))
        {
            throw new ConflictException("This email is already on the waitlist");
        }

        var vertical = Enum.Parse<Vertical>(request.PrimaryVertical, ignoreCase: true);

        var entry = new WaitlistEntry
        {
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            GamerTag = string.IsNullOrWhiteSpace(request.GamerTag) ? null : request.GamerTag.Trim(),
            PrimaryVertical = vertical,
            ReferralCode = string.IsNullOrWhiteSpace(request.ReferralCode) ? null : request.ReferralCode.Trim()
        };

        await _repository.AddAsync(entry, ct);

        // Confirmation email must not block or fail the signup itself - Brevo being down
        // shouldn't turn a successful waitlist join into a 500.
        try
        {
            await _emailService.SendWaitlistConfirmationAsync(entry.Email, entry.FullName, ct);
            entry.ConfirmationEmailSent = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send waitlist confirmation email to {Email}", entry.Email);
        }

        return new WaitlistSignupResponse
        {
            Id = entry.Id,
            Email = entry.Email,
            Message = "You're on the list! Check your email for confirmation."
        };
    }

    public async Task<WaitlistCountResponse> GetCountAsync(CancellationToken ct = default)
    {
        var count = await _repository.CountAsync(ct);
        return new WaitlistCountResponse { Count = count };
    }

    public async Task<WaitlistExistsResponse> CheckExistsAsync(string email, CancellationToken ct = default)
    {
        var exists = await _repository.ExistsByEmailAsync(email.Trim().ToLowerInvariant(), ct);
        return new WaitlistExistsResponse { Exists = exists };
    }

    public async Task<PagedResult<WaitlistAdminEntryDto>> GetAdminListAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var paged = await _repository.GetPagedAsync(page, pageSize, ct);

        return new PagedResult<WaitlistAdminEntryDto>
        {
            Total = paged.Total,
            Page = paged.Page,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(e => new WaitlistAdminEntryDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                GamerTag = e.GamerTag,
                PrimaryVertical = e.PrimaryVertical.ToString(),
                ReferralCode = e.ReferralCode,
                ConfirmationEmailSent = e.ConfirmationEmailSent,
                CreatedAt = e.CreatedAt
            }).ToList()
        };
    }

    public async Task DeleteEntryAsync(Guid id, CancellationToken ct = default)
    {
        var entry = await _repository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException("Waitlist entry", id);

        await _repository.DeleteAsync(entry, ct);
    }
}
