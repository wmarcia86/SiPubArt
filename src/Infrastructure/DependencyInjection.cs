using Application.Common.Authentication;
using Application.Common.Data;
using Domain.Articles;
using Domain.Comments;
using Domain.Core;
using Domain.Users;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

/// <summary>
/// Configuration class. Registers infrastructure services and dependencies.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services in the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddPersistence(configuration);

        services.AddTokenAuthentication(configuration);

        return services;
    }

    /// <summary>
    /// Registers JWT token authentication services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        if (jwtSettings == null || !jwtSettings.Exists())
        {
            throw new InvalidOperationException("La sección JwtSettings no está configurada en appsettings.");
        }

        var issuer = jwtSettings["Issuer"];

        if (string.IsNullOrEmpty(issuer))
        {
            throw new InvalidOperationException("JwtSettings:Issuer is not configured.");
        }

        var audience = jwtSettings["Audience"];

        if (string.IsNullOrEmpty(issuer))
        {
            throw new InvalidOperationException("JwtSettings:Audience is not configured.");
        }

        var secret = jwtSettings["Secret"];

        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JwtSettings:Secret is not configured.");
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });

        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }

    /// <summary>
    /// Registers persistence and repository services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlite(configuration.GetConnectionString("Sqlite")));

        services.AddScoped<IApplicationDbContext>(sp => 
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(sp => 
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }
}
