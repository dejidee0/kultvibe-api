using KultVibe.Domain.Entities;
using KultVibe.Shared.Wrappers;

namespace KultVibe.Application.Interfaces;

public interface IWaitlistRepository
{
    Task<WaitlistEntry?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<WaitlistEntry?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(WaitlistEntry entry, CancellationToken ct = default);
    Task DeleteAsync(WaitlistEntry entry, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<PagedResult<WaitlistEntry>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
}
