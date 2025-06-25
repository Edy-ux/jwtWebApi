
using jwtWebApi.Models;

namespace jwtWebApi.Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);

        public RefreshToken GenerateRefreshToken(string ipAddress, User user);
    }
}
