

using System.Text.Json.Serialization;

namespace jwtWebApi.Dto.User
{
    public class UserDtoResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string[]? Roles { get; set; } = [];
        public string? Email { get; set; } = string.Empty;
        public List<RefreshTokenDto>? RefreshTokens { get; set; }

    }
}