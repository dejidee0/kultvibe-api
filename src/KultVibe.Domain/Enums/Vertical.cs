namespace KultVibe.Domain.Enums;

/// <summary>
/// The six KultVibe verticals. String values here are locked - they are the exact
/// tokens the API accepts/returns and must stay in sync with the frontend.
/// Master Plan v2.0, Phase 1 Error Cases: "Primary vertical must be one of:
/// Gaming, Tech, Music, Lifestyle, Streaming, Creator".
/// </summary>
public enum Vertical
{
    Gaming = 0,
    Tech = 1,
    Music = 2,
    Lifestyle = 3,
    Streaming = 4,
    Creator = 5
}
