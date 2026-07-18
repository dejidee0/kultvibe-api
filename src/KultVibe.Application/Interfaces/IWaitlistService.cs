using KultVibe.Application.DTOs.Waitlist;
using KultVibe.Shared.Wrappers;

namespace KultVibe.Application.Interfaces;

public interface IWaitlistService
{
    Task<WaitlistSignupResponse> SignupAsync(WaitlistSignupRequest request, CancellationToken ct = default);
    Task<WaitlistCountResponse> GetCountAsync(CancellationToken ct = default);
    Task<WaitlistExistsResponse> CheckExistsAsync(string email, CancellationToken ct = default);
    Task<PagedResult<WaitlistAdminEntryDto>> GetAdminListAsync(int page, int pageSize, CancellationToken ct = default);
    Task DeleteEntryAsync(Guid id, CancellationToken ct = default);
}
