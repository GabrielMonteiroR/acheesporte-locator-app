using acheesporte_locator_app.Views;

namespace acheesporte_locator_app;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Services = serviceProvider;

        MainPage = new NavigationPage(App.Services.GetService<SplashPage>());

    }
}