using App.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Application.Extensions;

/// <summary>
/// EXTENSION METHODS FOR REGISTERING APPLICATION SERVICES.
/// </summary>
public static class ApplicationServiceExtension
{
    /// <summary>
    /// REGISTERS MEDIATR, VALIDATORS, AND PIPELINE BEHAVIORS.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // REGISTER MEDIATR
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);

            // REGISTER PIPELINE BEHAVIORS - ORDER MATTERS!
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // REGISTER ALL VALIDATORS FROM APPLICATION ASSEMBLY
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
