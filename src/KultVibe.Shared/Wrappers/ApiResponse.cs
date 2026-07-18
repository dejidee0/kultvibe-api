namespace KultVibe.Shared.Wrappers;

/// <summary>
/// Standard response envelope. EVERY KultVibe API endpoint returns this exact shape.
/// Master Plan v2.0, Section 2.1 - no exceptions, frontend always checks `success` first.
/// </summary>
/// <typeparam name="T">Type of the payload returned in <see cref="Data"/>.</typeparam>
public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string Message { get; init; } = string.Empty;
    public List<string>? Errors { get; init; }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
        => new()
        {
            Success = true,
            Data = data,
            Message = message,
            Errors = null
        };

    public static ApiResponse<T> FailureResponse(string message, List<string>? errors = null)
        => new()
        {
            Success = false,
            Data = default,
            Message = message,
            Errors = errors
        };

    public static ApiResponse<T> FailureResponse(string message, string singleError)
        => FailureResponse(message, new List<string> { singleError });
}

/// <summary>
/// Non-generic convenience factory for responses that carry no payload (e.g. DELETE endpoints).
/// </summary>
public static class ApiResponse
{
    public static ApiResponse<object?> SuccessResponse(string message = "Operation successful")
        => ApiResponse<object?>.SuccessResponse(null, message);

    public static ApiResponse<object?> FailureResponse(string message, List<string>? errors = null)
        => ApiResponse<object?>.FailureResponse(message, errors);
}
