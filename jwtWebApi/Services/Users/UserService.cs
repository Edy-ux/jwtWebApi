
using System.Linq.Expressions;
using AutoMapper;
using jwtWebApi.Application.Interfaces;
using jwtWebApi.Context;
using jwtWebApi.Dto;
using jwtWebApi.Dto.User;
using jwtWebApi.Exceptions;
using jwtWebApi.Models;
using JwtWebApi.Dto;
using Microsoft.EntityFrameworkCore;


namespace jwtWebApi.Services.Users;


public class UserService(IDbContextFactory<AppDbContext> context, ITokenService tokenService, IMapper mapper) : IUserService
{

    private readonly IDbContextFactory<AppDbContext> _contextFactory = context;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDtoResponse?> GetUserByIdAsync(int id)
    {
        using var context = _contextFactory.CreateDbContext();

        var entity = await context.Users.FindAsync(id);
        return _mapper.Map<UserDtoResponse>(entity);
    }
    public async Task<UserDtoResponse?> GetUserByLogin(UserDtoLogin? user)
    {

        using var context = _contextFactory.CreateDbContext();
        var entity = await context.Users
            .FirstOrDefaultAsync(u => u.Login == user.Login || u.UserName == user.UserName);

        if (entity == null)
            return null;
        if (!BCrypt.Net.BCrypt.Verify(user.Password, entity.PasswordHash))
            throw new UnauthorizedAccessException("Login or password Invalid");

        return _mapper.Map<UserDtoResponse>(entity);
    }
    public async Task<UserDtoResponse?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        using var context = _contextFactory.CreateDbContext();

        var user = await context.Users
            .Include(user => user.RefreshTokens.Where(rf => rf.Token.Equals(refreshToken) && !rf.Revoked))
            .FirstOrDefaultAsync(user => user.RefreshTokens.Any(rf => rf.Token.Equals(refreshToken) && !rf.Revoked));


        return _mapper.Map<UserDtoResponse>(user);

    }

    public async Task<(string accessToken, string refreshToken)> AuthenticateAsync(string login, string password, string ipAddress)
    {
        using var context = _contextFactory.CreateDbContext();

        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Login == login);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid login.");

        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(ipAddress, user);

        user.AddRefreshToken(refreshToken);

        await context.SaveChangesAsync();

        return (accessToken, refreshToken.Token);
    }

    public async Task<RefreshRequestDto?> RenewAccessTokenAsync(string refreshToken, string ipAddress)
    {
        using var context = _contextFactory.CreateDbContext();

        var tokenEntity = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(IsActiveRefreshToken(refreshToken));

        if (tokenEntity == null || tokenEntity.Expires < DateTime.UtcNow)
            return null;

        var user = tokenEntity.User;
        var newAccessToken = _tokenService.GenerateToken(user);

        /* if (tokenEntity.CreatedByIp != ipAddress)
            throw new UnauthorizedAccessException("IP address mismatch."); */

        var newRefreshToken = _tokenService.GenerateRefreshToken(ipAddress, user);

        user.RevokeRefreshToken(refreshToken);

        user.AddRefreshToken(newRefreshToken);

        await context.SaveChangesAsync();

        return new RefreshRequestDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        using var context = _contextFactory.CreateDbContext();

        var token = await context.RefreshTokens
             .FirstOrDefaultAsync(IsActiveRefreshToken(refreshToken));

        if (token == null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        token.Revoke();
        await context.SaveChangesAsync();

    }
    public async Task<UserDtoResponse?> InsertUserAsync(UserDto userDto)
    {
        using var context = _contextFactory.CreateDbContext();

        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.Login == userDto.Login || u.UserName == userDto.UserName);

        if (existingUser is not null)
        {
            throw new UserAlreadyExistsException($"User with login {userDto.Login} or {userDto.UserName}  already exists.");
        }
        var entity = new User(
            login: userDto.Login,
            email: userDto.Email,
            roles: userDto.Roles,
            username: userDto.UserName,
            hash: BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            );
        await context.Users.AddAsync(entity);

        await context.SaveChangesAsync();

        return _mapper.Map<UserDtoResponse>(entity);
    }
    public async Task UpdateUserAsync(User user)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }
    public async Task DeleteUserAsync(Guid id)
    {
        using var context = _contextFactory.CreateDbContext();
        var user = await context.Users.FindAsync(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
    private static Expression<Func<RefreshToken, bool>> IsActiveRefreshToken(string token) =>
           rt => rt.Token.Equals(token) && !rt.Revoked && rt.Expires > DateTime.UtcNow;

}