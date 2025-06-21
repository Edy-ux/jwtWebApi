using jwtWebApi.Configuration;
using jwtWebApi.Models;
using JwtWebApi.Controllers;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
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
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
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

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress), "IP address cannot be null or empty.");
            if (ipAddress == "::1") ipAddress = "127.0.0.1";

            byte[] randomBytes = GenerateRandomBytes();

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress

            };
        }
        private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32 = false) =>
              Encoding.UTF8.GetBytes(secret);

        private static byte[] GenerateRandomBytes(byte length = 64)
        {

            if (length > 255)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be less than or equal to 255.");

            var randomBytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return randomBytes;
        }

    }
}