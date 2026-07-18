using FluentValidation;
using KultVibe.Application.Interfaces;
using KultVibe.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KultVibe.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<DependencyInjection>();
        services.AddScoped<IWaitlistService, WaitlistService>();

        return services;
    }
}
