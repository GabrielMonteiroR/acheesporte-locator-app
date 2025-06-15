using acheesporte_locator_app.Interfaces;
using acheesporte_locator_app.Helpers;

namespace acheesporte_locator_app.Views;

public partial class SplashPage : ContentPage
{
    private readonly IUserInterface _userInterface;

    public SplashPage(IUserInterface userService)
    {
        InitializeComponent();
        _userInterface = userService;

        LoadGif();
    }

    private void LoadGif()
    {
        string html = @"
<html>
  <body style='margin:0;padding:0;background:transparent;display:flex;justify-content:center;align-items:center;height:100vh;'>
    <img src='file:///android_asset/loading.gif' width='200' height='200'/>
  </body>
</html>";
        GifWebView.Source = new HtmlWebViewSource { Html = html };

    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var user = await _userInterface.GetCurrentUserAsync();

            if (user != null)
            {
                UserSession.CurrentUser = user;
                Application.Current.MainPage = App.Services.GetService<AppShell>();
                await Shell.Current.GoToAsync("//MainApp/HomePage");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        catch
        {
            await Task.Delay(1500);
            await Navigation.PushAsync(App.Services.GetService<LoginPage>());

        }
    }
}