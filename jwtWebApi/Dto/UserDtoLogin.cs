
using jwtWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace JwtWebApi.Dto
{
    public class UserDtoLogin
    {

        public string? Login { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string? UserName { get; set; }

    }

}