
using System.Text.Json.Serialization;
using jwtWebApi.Dto.User;

namespace jwtWebApi.Dto;

public class RefreshTokenDto
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool? Revoked { get; set; }
    public DateTime RevokedAt { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public Guid UserId { get; set; }

    [JsonIgnore]
    public UserDtoResponse? User { get; set; }
}