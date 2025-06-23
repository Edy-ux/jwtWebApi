

namespace jwtWebApi.Dto;

public class UserDtoResponse
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string[]? Roles { get; set; }
    public List<RefreshTokenDto>? RefreshTokens { get; set; }
}
