using Domain.Common;
using Domain.Users.ValueObjects;

namespace Domain.Users;

/// <summary>
/// Repository interface. Defines data access operations for User entities.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public interface IUserRepository : IRepository<User, UserId>
{
    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">The username value object.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByUsernameAsync(Username username);

    /// <summary>
    /// Retrieves a user by email.
    /// </summary>
    /// <param name="username">The email value object.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    Task<User?> GetByEmailAsync(Email username);
}