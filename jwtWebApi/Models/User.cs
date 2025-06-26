namespace jwtWebApi.Models;

public class User
{
    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    public Guid Id { get; private set; }
    public string Login { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string[]? Roles { get; private set; }
    public string Email { get; private set; }

    // Construtor protegido para o EF
    protected User() { }
    public User(string login, string username, string hash, string email, string[] roles)
    {
        if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Login is Required");
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("UserName is Required");
        if (string.IsNullOrWhiteSpace(hash)) throw new ArgumentException("Senha is Required");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is Required");

        Id = Guid.NewGuid();
        Login = login;
        UserName = username;
        PasswordHash = hash;
        Email = email;
        Roles = roles ?? Array.Empty<string>();
        EmailConfirmed = false;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        if (refreshToken == null)
            throw new ArgumentNullException(nameof(refreshToken));
        // Exemplo de invariante: não permitir mais de 5 tokens ativos e/ou expirados

        if (_refreshTokens.Count(rt => !rt.IsExpired || !rt.Revoked) >= 5)
        {
            var oldestToken = _refreshTokens.OrderBy(rt => rt.Created).First();
            _refreshTokens.Remove(oldestToken);

            // throw new InvalidOperationException("Refresh tokens limit reached");
        }
        _refreshTokens.Add(refreshToken);

    }

    public void RevokeRefreshToken(string token)
    {
        var rt = _refreshTokens.FirstOrDefault(t => t.Token == token);
        if (rt == null)
        {
            throw new InvalidOperationException("Refresh token não encontrado.");
        }

        rt.Revoke();
    }

    public void ConfirmEmail() => EmailConfirmed = true;

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Nova senha obrigatória");
        PasswordHash = newPasswordHash;
    }
}