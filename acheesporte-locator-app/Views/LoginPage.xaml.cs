using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetService<LoginViewModel>();
    }

}