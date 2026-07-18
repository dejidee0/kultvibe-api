using Microsoft.AspNetCore.Authentication;

namespace KultVibe.API.Auth;

/// <summary>
/// Credentials for the Phase 1 admin Basic Auth scheme. This is a deliberately temporary
/// gate - Master Plan Phase 2 replaces it with real JWT + Role=Admin authorization once
/// the Users/Auth system exists. Bound from the "Admin" config section:
///   Admin__Username, Admin__Password (env vars, not in the Master Plan 2.5 table - add
///   them alongside AdminEmail when configuring Railway).
/// </summary>
public class AdminBasicAuthOptions : AuthenticationSchemeOptions
{
    public const string SectionName = "Admin";

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
