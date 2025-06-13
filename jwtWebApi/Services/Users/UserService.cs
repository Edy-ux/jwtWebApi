
using jwtWebApi.Application.Interfaces;
using jwtWebApi.Models;
using Microsoft.EntityFrameworkCore;
using TypesfRelationships.Context;

namespace jwtWebApi.Services.Users
{
    public class UserService(AppDbContext context) : IUserService
    {
        private readonly AppDbContext _context = context;

        public Task<User?> GetUserByLogin(string login)
        {
            throw new NotImplementedException();
        }

        // ...existing code...
        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
        }
        // ...existing code...


        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }

}