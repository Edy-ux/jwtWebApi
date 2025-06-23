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
    public User(string login, string userName, string passwordHash, string email, string[] roles)
    {
        if (string.IsNullOrWhiteSpace(login)) throw new ArgumentException("Login obrigatório");
        if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("UserName obrigatório");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Senha obrigatória");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email obrigatório");

        Id = Guid.NewGuid();
        Login = login;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        Roles = roles ?? Array.Empty<string>();
        EmailConfirmed = false;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        if (refreshToken == null)
            throw new ArgumentNullException(nameof(refreshToken));

        // Exemplo de invariante: não permitir mais de 5 tokens ativos
        if (_refreshTokens.Count(rt => !rt.IsExpired) >= 5)
            throw new InvalidOperationException("Limite de refresh tokens ativos atingido.");

        _refreshTokens.Add(refreshToken);
    }

    public void RevokeRefreshToken(string token)
    {
        var rt = _refreshTokens.FirstOrDefault(t => t.Token == token);
        if (rt == null)
            throw new InvalidOperationException("Refresh token não encontrado.");

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