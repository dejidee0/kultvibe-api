using KultVibe.Application.DTOs.Waitlist;
using KultVibe.Application.Interfaces;
using KultVibe.Shared.Constants;
using KultVibe.Shared.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KultVibe.API.Controllers;

[ApiController]
[Route("api/waitlist")]
public class WaitlistController : ControllerBase
{
    private readonly IWaitlistService _waitlistService;

    public WaitlistController(IWaitlistService waitlistService)
    {
        _waitlistService = waitlistService;
    }

    /// <summary>Save a waitlist signup and send the confirmation email via Brevo.</summary>
    [HttpPost]
    [EnableRateLimiting(RateLimitPolicies.WaitlistSignup)]
    [ProducesResponseType(typeof(ApiResponse<WaitlistSignupResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Signup([FromBody] WaitlistSignupRequest request, CancellationToken ct)
    {
        var result = await _waitlistService.SignupAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<WaitlistSignupResponse>.SuccessResponse(result, result.Message));
    }

    /// <summary>Total signup count. Public - used for homepage social proof stats bar.</summary>
    [HttpGet("count")]
    [ProducesResponseType(typeof(ApiResponse<WaitlistCountResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCount(CancellationToken ct)
    {
        var result = await _waitlistService.GetCountAsync(ct);
        return Ok(ApiResponse<WaitlistCountResponse>.SuccessResponse(result));
    }

    /// <summary>Check whether an email is already on the waitlist.</summary>
    [HttpGet("exists/{email}")]
    [ProducesResponseType(typeof(ApiResponse<WaitlistExistsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckExists(string email, CancellationToken ct)
    {
        var result = await _waitlistService.CheckExistsAsync(email, ct);
        return Ok(ApiResponse<WaitlistExistsResponse>.SuccessResponse(result));
    }
}
