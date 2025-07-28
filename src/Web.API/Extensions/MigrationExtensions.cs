using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;

namespace Web.API.Extensions;

/// <summary>
/// Static class providing extension methods for database migration operations.
/// Type: Extension
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Applies any pending database migrations for the application's DbContext.
    /// </summary>
    /// <param name="app">The WebApplication instance to apply migrations on.</param>
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}
