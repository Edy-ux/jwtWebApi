using jwtWebApi.Configuration;
using jwtWebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jwtWebApi.Services.Token
{
    public class JWTTokenService(IOptions<ConfigurationOptions> options) : ITokenService
    {

        private readonly ConfigurationOptions _options = options.Value;

        public string GenerateToken(User user)
        {
            var handle = new JwtSecurityTokenHandler();

            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)

            ];

            foreach (var role in user.Roles!)
                claims.Add(new Claim(ClaimTypes.Role, role));



            var key = new SymmetricSecurityKey(ConvertSecretToBytes(
                _options.Secret_Key));

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: claims,
                notBefore: DateTime.Now.AddSeconds(5),
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var jwt = handle.WriteToken(token);

            return jwt;
        }

        private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32 = false) =>
              Encoding.UTF8.GetBytes(secret);
    }
}