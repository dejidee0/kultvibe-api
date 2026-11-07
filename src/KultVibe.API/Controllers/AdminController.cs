using KultVibe.API.Auth;
using KultVibe.Application.DTOs.Waitlist;
using KultVibe.Application.Interfaces;
using KultVibe.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KultVibe.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(AuthenticationSchemes = AdminBasicAuthHandler.SchemeName)]
public class AdminController : ControllerBase
{
    private readonly IWaitlistService _waitlistService;

    public AdminController(IWaitlistService waitlistService)
    {
        _waitlistService = waitlistService;
    }

    /// <summary>All waitlist entries, paginated. CEO only.</summary>
    [HttpGet("waitlist")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<WaitlistAdminEntryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitlist([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _waitlistService.GetAdminListAsync(page, pageSize, ct);
        return Ok(ApiResponse<PagedResult<WaitlistAdminEntryDto>>.SuccessResponse(result));
    }

    /// <summary>Waitlist totals by user type, interest, game and legacy category. CEO only.</summary>
    [HttpGet("waitlist/summary")]
    [ProducesResponseType(typeof(ApiResponse<WaitlistSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWaitlistSummary(CancellationToken ct)
    {
        var result = await _waitlistService.GetSummaryAsync(ct);
        return Ok(ApiResponse<WaitlistSummaryResponse>.SuccessResponse(result));
    }

    /// <summary>Remove a waitlist entry. CEO only.</summary>
    [HttpDelete("waitlist/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWaitlistEntry(Guid id, CancellationToken ct)
    {
        await _waitlistService.DeleteEntryAsync(id, ct);
        return Ok(ApiResponse.SuccessResponse("Waitlist entry removed"));
    }
}
