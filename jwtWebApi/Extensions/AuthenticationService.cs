
using System.Text;
using jwtWebApi.Application.Interfaces;
using jwtWebApi.Services.Token;
using jwtWebApi.Services.Users;

namespace jwtWebApi.Extensions;

public static class AuthenticationService
{

    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ITokenService, JWTTokenService>();
        services.AddScoped<IUserService, UserService>();

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
