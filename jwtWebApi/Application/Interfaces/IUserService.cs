

using jwtWebApi.Models;

namespace jwtWebApi.Application.Interfaces
{
    public interface IUserService
    {

        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task UpdateUserAsync(User user);
        Task<User?> GetUserByLogin(string login);

    }
}

