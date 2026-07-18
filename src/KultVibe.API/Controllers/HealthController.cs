using KultVibe.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KultVibe.API.Controllers;

[ApiController]
[Route("api/health")]
[AllowAnonymous]
public class HealthController : ControllerBase
{
    /// <summary>Health check. Railway polls this to determine container liveness.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(ApiResponse<object>.SuccessResponse(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow
        }));
    }
}
