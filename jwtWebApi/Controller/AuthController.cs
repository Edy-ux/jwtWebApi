using JwtWepApi.NET.Dto;
using JwtWepApi.NET.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtWepApi.NET.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {


        public static User user = new();
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {

            string pw_hash
                   = BCrypt.Net.BCrypt.HashPassword(request.Password);


            user.Username = request.Username;
            user.PasswordHash = pw_hash;

            return Ok(user);

        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest("User Not Found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong is Passoword");
            }

            string token = CreateToken(user);

            return Ok(token);

        }

        [NonAction]
        public string CreateToken(User user)
        {


            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var token = new JwtSecurityToken(
                  issuer: user.Username,
                  claims: claims,
                  expires: DateTime.Now.AddDays(1),
                  signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)

                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }



    }

}
