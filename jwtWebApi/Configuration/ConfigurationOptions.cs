namespace jwtWebApi.Configuration;

public class ConfigurationOptions
{
    public const string JWT = "JWT";
    public string Secret_Key { get; set; } = String.Empty;
    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;

}
