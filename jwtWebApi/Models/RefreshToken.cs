using System.Text.Json.Serialization;

namespace jwtWebApi.Models;



public class RefreshToken
{

    public Guid Id { get; private set; }
    public string Token { get; private set; }
    public DateTime Expires { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; private set; }
    public string CreatedByIp { get; private set; }
    public Guid UserId { get; private set; }

    [JsonIgnore]
    public User User { get; private set; }  // Navegação para o usuário associado ao token
    public bool? IsActive => !Revoked && !IsExpired; // 
    public bool Revoked { get; private set; } = false;
    public DateTime? RevokedAt { get; private set; }
    public RefreshToken(string token, DateTime expires, string createdByIp, Guid userId)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        Created = DateTime.UtcNow;
        CreatedByIp = createdByIp;
        Revoked = false;
    }

    protected RefreshToken() { } // Para EF Core

    public void Revoke()
    {
        Revoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}