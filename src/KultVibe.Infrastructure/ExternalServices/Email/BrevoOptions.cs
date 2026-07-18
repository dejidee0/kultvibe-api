namespace KultVibe.Infrastructure.ExternalServices.Email;

/// <summary>Bound from the "Brevo" config section - Brevo__ApiKey / Brevo__FromEmail env vars per Master Plan 2.5.</summary>
public class BrevoOptions
{
    public const string SectionName = "Brevo";

    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "KultVibe";
}
