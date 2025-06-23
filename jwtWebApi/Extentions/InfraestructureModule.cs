
using jwtWebApi.Configuration;
using jwtWebApi.Context;
using Microsoft.EntityFrameworkCore;

namespace jwtWebApi.Extentions
{
    public static class InfraestructureModule
    {

        public static IServiceCollection AddInfraestructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.


            services.AddApiInternacionalization()
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