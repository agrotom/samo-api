using Samo.Models;

namespace Samo.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}