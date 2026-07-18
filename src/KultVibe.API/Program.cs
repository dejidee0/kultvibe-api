using System.Threading.RateLimiting;
using KultVibe.API.Auth;
using KultVibe.API.Middleware;
using KultVibe.Application;
using KultVibe.Infrastructure;
using KultVibe.Infrastructure.Persistence;
using KultVibe.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---- Layer registration (Clean Architecture composition root) ----
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ---- Admin Basic Auth (Phase 1 only - replaced by JWT/Role=Admin in Phase 2) ----
builder.Services.Configure<AdminBasicAuthOptions>(builder.Configuration.GetSection(AdminBasicAuthOptions.SectionName));
builder.Services
    .AddAuthentication(AdminBasicAuthHandler.SchemeName)
    .AddScheme<AdminBasicAuthOptions, AdminBasicAuthHandler>(AdminBasicAuthHandler.SchemeName, _ => { });
builder.Services.AddAuthorization();

// ---- CORS: Master Plan 2.5, CORS__AllowedOrigins ----
const string corsPolicyName = "KultVibeFrontend";
var allowedOrigins = builder.Configuration["CORS:AllowedOrigins"]?
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            // No origins configured (e.g. first local run) - fall back to permissive dev-only policy.
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    });
});

// ---- Rate limiting: 429 on repeated waitlist signups (Master Plan Phase 1 Error Cases) ----
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter(RateLimitPolicies.WaitlistSignup, opts =>
    {
        opts.PermitLimit = 5;
        opts.Window = TimeSpan.FromHours(1);
        opts.QueueLimit = 0;
        opts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// ---- Controllers, Swagger ----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "KultVibe API",
        Version = "v1",
        Description = "Africa's competitive creator platform - Phase 1: Public Platform + Waitlist"
    });

    c.AddSecurityDefinition("basic", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "basic",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Basic auth for /api/admin/* endpoints (Phase 1 only)"
    });
});

var app = builder.Build();

// ---- Apply pending EF Core migrations on startup ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// ---- Middleware pipeline ----
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Exposed for WebApplicationFactory-based integration tests.
public partial class Program { }
