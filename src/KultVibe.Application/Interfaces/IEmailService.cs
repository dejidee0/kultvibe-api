namespace KultVibe.Application.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// Sends the waitlist confirmation email via Brevo. Master Plan Phase 1 Definition of
    /// Done: "Confirmation email arrives within 30 seconds of signup".
    /// </summary>
    Task SendWaitlistConfirmationAsync(string toEmail, string toName, CancellationToken ct = default);
}
