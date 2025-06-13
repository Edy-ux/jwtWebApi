using jwtWebApi.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace jwtWebApi.Extentions;

public static class AuthenticationService
{

    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("JWT:Audience").Value!,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                   configuration.GetSection("JWT:Secret_Key").Value!)
                    )
            };
        });
        return services;
    }


}
