using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Web.API.Middlewares;

namespace Web.API;

/// <summary>
/// Configuration class. Registers presentation layer services and middleware.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers controllers, OData, Swagger, and global exception handling middleware for the presentation layer.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers().AddOData(options =>
        {
            options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
        });

        services.AddEndpointsApiExplorer();
        
        //services.AddSwaggerGen();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "1.0" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese el token JWT en este formato: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        services.AddTransient<GloblalExceptionHandlingMiddleware>();

        return services;
    }
}