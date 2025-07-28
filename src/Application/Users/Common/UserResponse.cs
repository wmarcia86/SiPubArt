namespace Application.Users.Common;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Username,
    string Email,
    bool Active);