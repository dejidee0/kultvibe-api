using KultVibe.Application.Interfaces;
using KultVibe.Domain.Entities;
using KultVibe.Infrastructure.Persistence;
using KultVibe.Shared.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace KultVibe.Infrastructure.Repositories;

public class WaitlistRepository : IWaitlistRepository
{
    private readonly ApplicationDbContext _db;

    public WaitlistRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<WaitlistEntry?> GetByEmailAsync(string email, CancellationToken ct = default)
        => _db.WaitlistEntries.FirstOrDefaultAsync(w => w.Email == email, ct);

    public Task<WaitlistEntry?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.WaitlistEntries.FirstOrDefaultAsync(w => w.Id == id, ct);

    public async Task AddAsync(WaitlistEntry entry, CancellationToken ct = default)
    {
        await _db.WaitlistEntries.AddAsync(entry, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(WaitlistEntry entry, CancellationToken ct = default)
    {
        _db.WaitlistEntries.Remove(entry);
        await _db.SaveChangesAsync(ct);
    }

    public Task<int> CountAsync(CancellationToken ct = default)
        => _db.WaitlistEntries.CountAsync(ct);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        => _db.WaitlistEntries.AnyAsync(w => w.Email == email, ct);

    public async Task<PagedResult<WaitlistEntry>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query = _db.WaitlistEntries.AsNoTracking().OrderByDescending(w => w.CreatedAt);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<WaitlistEntry>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}
