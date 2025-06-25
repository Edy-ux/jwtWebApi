

using jwtWebApi.Dto;
using jwtWebApi.Dto.User;
using jwtWebApi.Models;

namespace jwtWebApi.Application.Interfaces
{
    public interface IUserService
    {

        Task<UserDtoResponse?> GetUserByIdAsync(int id);
        Task<User?> GetUserByLogin(string login);
        Task UpdateUserAsync(User user);
        Task<User?> InsertUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<UserDtoResponse?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<(string accessToken, string refreshToken)> AuthenticateAsync(string login, string password, string ipAddress);
        Task<RefreshRequestDto?> RenewAccessTokenAsync(string refreshToken, string ipAddress);
        Task LogoutAsync(string refreshToken);

    }
}

