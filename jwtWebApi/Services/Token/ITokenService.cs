
namespace jwtWebApi.Services.Token
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
