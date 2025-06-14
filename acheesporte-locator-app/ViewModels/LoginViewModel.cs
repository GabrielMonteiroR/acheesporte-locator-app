using acheesporte_locator_app.Dtos;
using acheesporte_locator_app.Helpers;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace acheesporte_locator_app.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IUserInterface _userInterface;

    public LoginViewModel(IUserInterface userService)
    {
        _userInterface = userService;
    }

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;


    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Preencha o e-mail e a senha.", "OK");
                return;
            }

            var dto = new SignInRequestDto
            {
                Email = Email,
                Password = Password
            };

            await _userInterface.SignInUserAsync(dto);

            var currentUser = await _userInterface.GetCurrentUserAsync();

            if (currentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível carregar os dados do usuário.", "OK");
                return;
            }

            UserSession.CurrentUser = currentUser;


            Application.Current.MainPage = App.Services.GetService<AppShell>();
            await Shell.Current.GoToAsync("//MainApp/HomePage");
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "Email ou senha inválidos", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }


    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            await Application.Current.MainPage.Navigation.PushAsync(App.Services.GetService<RegisterPage>());

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }
}