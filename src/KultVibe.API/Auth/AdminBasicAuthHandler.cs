using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KultVibe.API.Auth;

/// <summary>
/// "Basic" authentication scheme for the /api/admin endpoints in Phase 1.
/// Compares against Admin__Username / Admin__Password, not a database - there is no
/// Users table yet. Superseded by JWT + Role=Admin in Phase 2 (see class doc on
/// AdminBasicAuthOptions).
/// </summary>
public class AdminBasicAuthHandler : AuthenticationHandler<AdminBasicAuthOptions>
{
    public const string SchemeName = "AdminBasic";

    public AdminBasicAuthHandler(
        IOptionsMonitor<AdminBasicAuthOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader) ||
            authHeader.ToString() is not { Length: > 0 } headerValue ||
            !headerValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Basic auth header"));
        }

        string username, password;
        try
        {
            var encoded = headerValue["Basic ".Length..].Trim();
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var parts = decoded.Split(':', 2);
            if (parts.Length != 2) return Task.FromResult(AuthenticateResult.Fail("Malformed Basic auth header"));
            (username, password) = (parts[0], parts[1]);
        }
        catch (FormatException)
        {
            return Task.FromResult(AuthenticateResult.Fail("Malformed Basic auth header"));
        }

        if (string.IsNullOrEmpty(Options.Username) || string.IsNullOrEmpty(Options.Password))
        {
            Logger.LogWarning("Admin Basic Auth credentials are not configured - denying all admin requests");
            return Task.FromResult(AuthenticateResult.Fail("Admin credentials not configured"));
        }

        var usernameMatches = CryptographicEquals(username, Options.Username);
        var passwordMatches = CryptographicEquals(password, Options.Password);

        if (!usernameMatches || !passwordMatches)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid admin credentials"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "Admin") };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.WWWAuthenticate = "Basic realm=\"KultVibe Admin\"";
        return base.HandleChallengeAsync(properties);
    }

    /// <summary>Constant-time string comparison to avoid timing side-channels on credential checks.</summary>
    private static bool CryptographicEquals(string a, string b)
    {
        var aBytes = Encoding.UTF8.GetBytes(a);
        var bBytes = Encoding.UTF8.GetBytes(b);
        return System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
    }
}
