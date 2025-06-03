using jwtWebApi.Configuration;
using jwtWebApi.Models;
using JwtWebApi.Controller;
using Microsoft.Extensions.Options;

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
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];

            foreach (var role in user.Roles!)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(ConvertSecretToBytes(
                _options.Secret_Key));

            var token = new JwtSecurityToken(
                issuer: nameof(AuthController),
                claims: claims,
                audience: _options.Audience,
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