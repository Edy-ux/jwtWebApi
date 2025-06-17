
using jwtWebApi.Application.Interfaces;
using jwtWebApi.Models;
using Microsoft.EntityFrameworkCore;
using jwtWebApi.Context;
using jwtWebApi.Exeptions;


namespace jwtWebApi.Services.Users;

public class UserService(IDbContextFactory<AppDbContext> context) : IUserService
{

    private readonly IDbContextFactory<AppDbContext> _contextFactory = context;


    public async Task<User?> GetUserByIdAsync(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByLogin(string login)
    {
        // Check if the login is null or empty
        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentException("Login cannot be null or empty.", nameof(login));
        }
        using var context = _contextFactory.CreateDbContext();
        return await context.Users
            .FirstOrDefaultAsync(u => u.Login == login);
    }


    // ...existing code...
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
    }

    public async Task<User?> InsertUserAsync(User user)
    {
        using var context = _contextFactory.CreateDbContext();

        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.Login == user.Login || u.Email == user.Email);

        if (existingUser is not null)
        {
            throw new UserAlreadyExistsException($"User with login '{user.Login}' already exists.");
        }

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }


    // ...existing code...
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
}