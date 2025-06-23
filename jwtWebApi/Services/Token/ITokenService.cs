
using jwtWebApi.Models;

namespace jwtWebApi.Services.Token
{
    public interface ITokenService
    {
        public string GenerateToken(User user);

        public RefreshToken GenerateRefreshToken(string ipAddress, User user);
    }
}
