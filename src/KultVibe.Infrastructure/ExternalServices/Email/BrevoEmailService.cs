using System.Net.Http.Headers;
using System.Net.Http.Json;
using KultVibe.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KultVibe.Infrastructure.ExternalServices.Email;

/// <summary>
/// Sends transactional email through Brevo's v3 REST API (https://api.brevo.com/v3/smtp/email).
/// Registered as a named/typed HttpClient in Infrastructure DependencyInjection.
/// </summary>
public class BrevoEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly BrevoOptions _options;
    private readonly ILogger<BrevoEmailService> _logger;

    public BrevoEmailService(HttpClient httpClient, IOptions<BrevoOptions> options, ILogger<BrevoEmailService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _httpClient.BaseAddress ??= new Uri("https://api.brevo.com/v3/");
        if (!string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            _httpClient.DefaultRequestHeaders.Remove("api-key");
            _httpClient.DefaultRequestHeaders.Add("api-key", _options.ApiKey);
        }
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task SendWaitlistConfirmationAsync(string toEmail, string toName, CancellationToken ct = default)
    {
        var payload = new
        {
            sender = new { email = _options.FromEmail, name = _options.FromName },
            to = new[] { new { email = toEmail, name = toName } },
            subject = "You're on the KultVibe waitlist \ud83d\udd25",
            htmlContent = $"""
                <p>Hey {toName},</p>
                <p>You're officially on the KultVibe waitlist. Your Culture. Your Stage. Your Money.</p>
                <p>We'll email you the moment your access opens up.</p>
                <p>&mdash; The KultVibe Team</p>
                """
        };

        var response = await _httpClient.PostAsJsonAsync("smtp/email", payload, ct);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            _logger.LogError("Brevo send failed ({StatusCode}) for {Email}: {Body}", response.StatusCode, toEmail, body);
            response.EnsureSuccessStatusCode();
        }
    }
}
