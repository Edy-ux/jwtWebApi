namespace jwtWebApi.Configuration;

public class ConfigurationOptions
{
    public const string JWT = "JWT";
    public string Secret_Key { get; set; } = String.Empty;
    public string Isuer { get; set; } = String.Empty;
}
