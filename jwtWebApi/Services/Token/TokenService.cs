using Google.Authenticator;

namespace jwtWebApi.Services.Token
{
    public class JWTTokenService(IConfiguration configuration) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
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
                _configuration.GetSection("AppSettings:Token").Value!, false));

            var token = new JwtSecurityToken(
                issuer: user.Username,
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