

using jwtWebApi.Models;

namespace jwtWebApi.Application.Interfaces
{
    public interface IUserService
    {

        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task UpdateUserAsync(User user);
        Task<User?> InsertUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByLogin(string login);

    }
}

