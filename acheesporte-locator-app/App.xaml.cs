using acheesporte_locator_app.Views;
using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Services = serviceProvider;

        var splash = App.Services.GetService<SplashPage>();
        MainPage = new NavigationPage(splash);

    }
}