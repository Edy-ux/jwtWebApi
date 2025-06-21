namespace jwtWebApi.Extentions;

public static class ApiInternacionalizationService
{
    public static IServiceCollection AddApiInternacionalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        return services;
    }

    public static WebApplication UseApiInternacionalization(this WebApplication app)
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
