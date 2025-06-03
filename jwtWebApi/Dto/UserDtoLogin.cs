
using System.ComponentModel.DataAnnotations;

namespace JwtWebApi.Dto
{
    public class UserDtoLogin
    {
        [Required(ErrorMessage = "Login is required.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }


    }
}