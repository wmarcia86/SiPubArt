using Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Configuration class. Registers application services and behaviors.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers application layer services, MediatR, validation, and AutoMapper.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
        });

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>)
        );

        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationAssemblyReference).Assembly));

        return services;
    }
}