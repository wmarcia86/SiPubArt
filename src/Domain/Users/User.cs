using Domain.Articles;
using Domain.Core;
using Domain.Users.ValueObjects;

namespace Domain.Users;

/// <summary>
/// Entity. Represents a user in the system.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class User : AggregateRoot
{
    public UserId Id { get; set; }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
    public Username Username { get; set; }
    public Email Email { get; set; }
    public Password Password { get; set; }
    public UserRole Role { get; set; }
    public bool Active { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Article> Articles { get; set; } = new List<Article>();

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the User class.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="username">The username.</param>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="role">The user's role.</param>
    /// <param name="active">Indicates if the user is active.</param>
    public User(
        UserId id,
        FirstName firstName,
        LastName lastName,
        Username username,
        Email email,
        Password password,
        UserRole role,
        bool active)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Role = role;
        Active = active;
    }
}