using Domain.Users;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository class. Implements data access operations for User entities.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the UserRepository class.
    /// </summary>
    /// <param name="context">The application's database context.</param>
    public UserRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user to add.</param>
    public void Add(User user) => 
        _context.Users.Add(user);

    /// <summary>
    /// Retrieves a user by its identifier.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetByIdAsync(UserId id) => 
        await _context.Users
            .SingleOrDefaultAsync(user => user.Id.Equals(id));

    /// <summary>
    /// Checks if a user exists by its identifier.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <returns>True if the user exists; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(UserId id) => 
        await _context.Users
            .AnyAsync(user => user.Id.Equals(id));

    /// <summary>
    /// Retrieves all users from the repository.
    /// </summary>
    /// <returns>A list of all users.</returns>
    public async Task<List<User>> GetAll() => 
        await _context.Users.ToListAsync();

    /// <summary>
    /// Gets the total number of users in the repository.
    /// </summary>
    /// <returns>The count of users.</returns>
    public async Task<int> Count() => 
        await _context.Users.CountAsync();

    /// <summary>
    /// Retrieves a paged list of users, ordered by identifier descending.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of users for the specified page.</returns>
    public async Task<List<User>> GetPaged(int pageNumber, int pageSize) =>
        await _context.Users
            .OrderByDescending(user => user.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    /// <summary>
    /// Updates an existing user in the repository.
    /// </summary>
    /// <param name="user">The user to update.</param>
    public void Update(User user) => 
        _context.Users.Update(user);

    /// <summary>
    /// Deletes a user from the repository.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    public void Delete(User user) => 
        _context.Users.Remove(user);

    /// <summary>
    /// Retrieves a user by username.
    /// </summary>
    /// <param name="username">The username value object.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetByUsernameAsync(Username username) => 
        await _context.Users
            .SingleOrDefaultAsync(user => user.Username == username);

    /// <summary>
    /// Retrieves a user by email.
    /// </summary>
    /// <param name="email">The email value object.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetByEmailAsync(Email email) =>
        await _context.Users
            .SingleOrDefaultAsync(user => user.Email == email);
}