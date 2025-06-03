using jwtWebApi.Models;

namespace jwtWebApi.Services;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = new User();
}