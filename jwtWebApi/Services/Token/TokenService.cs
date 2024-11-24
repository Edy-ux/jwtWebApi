using Google.Authenticator;
using jwtWebApi.Configuration;
using Microsoft.Extensions.Options;

namespace jwtWebApi.Services.Token
{
    public class JWTTokenService(IOptions<ConfigurationOptions> options) : ITokenService
    {

        private readonly ConfigurationOptions _options = options.Value;

        public string str = null!;
        public string GenerateToken(User user)
        {
            var handle = new JwtSecurityTokenHandler();

            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, user.Username),
                
            ];

            foreach (var role in user.Roles!)
                claims.Add(new Claim(ClaimTypes.Role, role));


            
            var key = new SymmetricSecurityKey(ConvertSecretToBytes(
                _options.Secret_Key, false));

            var token = new JwtSecurityToken(
                issuer: "localhost",
                claims: claims,
                notBefore: DateTime.Now.AddSeconds(5),
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var jwt = handle.WriteToken(token);

            return jwt;
        }

        private static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
              Encoding.UTF8.GetBytes(secret);
    }
}