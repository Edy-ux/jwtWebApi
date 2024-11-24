namespace jwtWebApi.Services.Auth;

public interface IAuthService
{
    public (string secretKey, string qrCodeUrl) GenerateTwoFactor(string email);

    public bool ValidateCode(string code, string key);
}
