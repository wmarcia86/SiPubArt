namespace Application.Users.Login;

public record LoggedUserResponse(
    Guid Id,
    string FullName,
    string Username,
    string Email,
    string Role,
    string Token);