

using jwtWebApi.Dto;
using jwtWebApi.Dto.User;
using jwtWebApi.Models;
using JwtWebApi.Dto;

namespace jwtWebApi.Application.Interfaces
{
    public interface IUserService
    {

        Task<UserDtoResponse?> GetUserByIdAsync(int id);
        Task<UserDtoResponse?> GetUserByLogin(UserDtoLogin user);
        Task UpdateUserAsync(User user);
        Task<UserDtoResponse?> InsertUserAsync(UserDto user);
        Task DeleteUserAsync(Guid id);
        Task<UserDtoResponse?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<(string accessToken, string refreshToken)> AuthenticateAsync(string login, string password, string ipAddress);
        Task<RefreshRequestDto?> RenewAccessTokenAsync(string refreshToken, string ipAddress);
        Task LogoutAsync(string refreshToken);

    }
}

