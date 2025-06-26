namespace jwtWebApi.Extensions;

public static class ApiInternationalizationService
{
    public static IServiceCollection AddApiInternationalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        return services;
    }

    public static WebApplication UseApiInternationalization(this WebApplication app)
    {
        var supportedCultures = new string[] { "pt-BR", "en-US" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture("pt-BR")
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);
        return app;
    }
}
