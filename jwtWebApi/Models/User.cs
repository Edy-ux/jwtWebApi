using System.ComponentModel.DataAnnotations;
namespace jwtWebApi.Models;

public class User
{

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string UserName { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public bool? EmailConfirmed { get; set; } = false;
    public string[]? Roles { get; set; } = [];
    public string Email { get; set; } = string.Empty;


    public void AddRefreshToken(RefreshToken refreshToken)
    {
        if (refreshToken == null)
            throw new ArgumentNullException(nameof(refreshToken));
        _refreshTokens.Add(refreshToken);
    }

}

