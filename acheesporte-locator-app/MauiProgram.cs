using acheesporte_athlete_app.Services;
using acheesporte_locator_app.Config;
using acheesporte_locator_app.Interfaces;
using acheesporte_locator_app.Services;
using acheesporte_locator_app.ViewModels;
using acheesporte_locator_app.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace acheesporte_locator_app;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("acheesporte_locator_app.appsettings.json");
        if (stream == null)
            throw new InvalidOperationException("Embedded resource 'acheesporte_locator_app.appsettings.json' not found.");
        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

        builder.Configuration.AddConfiguration(config);
        builder.Services.Configure<ApiSettings>(config.GetSection("ApiSettings"));
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<ApiSettings>>().Value);

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<SplashPage>();
        builder.Services.AddTransient<VenueViewModel>();
        builder.Services.AddTransient<VenueListPage>();
        builder.Services.AddTransient<VenueRegisterViewModel>();
        builder.Services.AddTransient<VenueRegisterViewModel>();

        builder.Services.AddHttpClient<VenueService>(client =>
        {
            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        builder.Services.AddHttpClient<IVenueService, VenueService>(client =>
        {
            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        builder.Services.AddHttpClient<ICepService, CepService>(client =>
        {
            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        builder.Services.AddHttpClient<IImageInterface, ImageService>(client =>
        {
            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        builder.Services.AddHttpClient<IUserInterface, UserService>(client =>
        {
            var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        builder
            .UseMauiApp<App>()
             .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
