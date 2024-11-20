using jwtWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace jwtWebApi.Services.UserService
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {

        private readonly IConfiguration _configuration = configuration;
        public string GenerateToken(User user)
        {

            var handle = new JwtSecurityTokenHandler();

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var token = new JwtSecurityToken(
                  issuer: user.Username,
                  claims: claims,
                  notBefore: DateTime.Now.AddSeconds(10),
                  expires: DateTime.Now.AddDays(1),
                  signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

                );

            var jwt = handle.WriteToken(token);

            return jwt;
        }
    }
}
