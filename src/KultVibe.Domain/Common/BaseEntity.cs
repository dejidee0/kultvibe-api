namespace KultVibe.Domain.Common;

/// <summary>
/// Base type for every domain entity. Guid PKs and UTC timestamps throughout,
/// matching the table definitions in Master Plan v2.0 Section 4.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
