
using jwtWebApi.Configuration;
using jwtWebApi.Context;
using Microsoft.EntityFrameworkCore;

namespace jwtWebApi.Extensions
{
    public static class InfrastructureModule
    {

        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.


            services.AddApiInternationalization()
                    .AddAuthenticationService(configuration)
                    .Configure<ConfigurationOptions>(configuration.GetSection(ConfigurationOptions.JWT))
                    .AddDbContextFactory<AppDbContext>(options =>
                     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                    .AddControllers()
                    .AddDataAnnotationsLocalization();

            return services;
        }
    }
}