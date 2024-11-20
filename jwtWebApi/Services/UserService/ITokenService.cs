using jwtWebApi.Models;

namespace jwtWebApi.Services.UserService
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
     