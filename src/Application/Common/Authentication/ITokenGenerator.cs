using Domain.Users;

namespace Application.Common.Authentication;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}