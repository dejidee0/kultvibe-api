namespace KultVibe.Domain.Exceptions;

/// <summary>
/// Base type for exceptions the API's exception-handling middleware knows how to translate
/// into an ApiResponse + HTTP status. Throw the specific subtype, never this directly.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

/// <summary>Maps to 404. "Tournament/user/match doesn't exist" per Master Plan 2.2.</summary>
public sealed class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} with identifier '{key}' was not found.") { }
}

/// <summary>Maps to 409. "Email already registered, already registered for tournament" per Master Plan 2.2.</summary>
public sealed class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}

/// <summary>
/// Maps to 400. Carries one or more field-level messages, matching the
/// { success:false, errors: [...] } shape from Master Plan 2.1.
/// </summary>
public sealed class ValidationException : DomainException
{
    public List<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ValidationException(List<string> errors) : base("Validation failed")
    {
        Errors = errors;
    }
}

/// <summary>Maps to 422. "Insufficient VC balance, tournament full, wrong ELO rank" per Master Plan 2.2.</summary>
public sealed class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message) { }
}
