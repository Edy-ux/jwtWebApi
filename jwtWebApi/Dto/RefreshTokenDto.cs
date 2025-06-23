
namespace jwtWebApi.Dto;

public class RefreshTokenDto
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool? IsActive { get; set; }
    public bool Revoked { get; set; }
    public DateTime Created { get; set; }
}