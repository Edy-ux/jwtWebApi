using Google.Authenticator;

namespace jwtWebApi.Services.Auth;

public class GoogleAuthService(TwoFactorAuthenticator tfa) : IAuthService
{
    private readonly TwoFactorAuthenticator _tfa = tfa;
    public (string secretKey, string qrCodeUrl) GenerateTwoFactor(string email)
    {
        string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);


        SetupCode setupInfo = _tfa.GenerateSetupCode("Traking Code (2FA)", email, key, false);

        Console.WriteLine(new { Key = key });

        return (key, setupInfo.QrCodeSetupImageUrl);
    }

    public bool ValidateCode(string code, string key)
    {
        return _tfa.ValidateTwoFactorPIN(key, code);
    }
}
