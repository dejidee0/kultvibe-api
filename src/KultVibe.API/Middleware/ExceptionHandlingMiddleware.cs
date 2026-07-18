using System.Net;
using System.Text.Json;
using KultVibe.Domain.Exceptions;
using KultVibe.Shared.Wrappers;

namespace KultVibe.API.Middleware;

/// <summary>
/// Catches DomainException subtypes thrown anywhere in the request pipeline and turns them
/// into the standard ApiResponse envelope with the matching HTTP status from Master Plan 2.2.
/// Anything unhandled becomes a 500 with no internal details leaked to the client.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            KultVibe.Domain.Exceptions.ValidationException ex =>
                (HttpStatusCode.BadRequest, ApiResponse.FailureResponse("Validation failed", ex.Errors)),

            ConflictException ex =>
                (HttpStatusCode.Conflict, ApiResponse.FailureResponse(ex.Message)),

            NotFoundException ex =>
                (HttpStatusCode.NotFound, ApiResponse.FailureResponse(ex.Message)),

            BusinessRuleException ex =>
                (HttpStatusCode.UnprocessableEntity, ApiResponse.FailureResponse(ex.Message)),

            _ => (HttpStatusCode.InternalServerError, ApiResponse.FailureResponse("An unexpected error occurred"))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
        }
        else
        {
            _logger.LogWarning("{ExceptionType}: {Message}", exception.GetType().Name, exception.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
}
