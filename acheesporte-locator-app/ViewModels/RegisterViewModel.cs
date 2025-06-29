using acheesporte_locator_app.Dtos;
using acheesporte_locator_app.Helpers;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flunt.Br.Extensions;
using Flunt.Extensions.Br.Validations;
using Flunt.Notifications;
using Flunt.Validations;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace acheesporte_locator_app.ViewModels;

public partial class RegisterViewModel : ObservableObject, INotifyDataErrorInfo
{
    private readonly IImageService _imageInterface;
    private readonly IUserService _userInterface;

    public RegisterViewModel(IImageService imageService, IUserService userService)
    {
        _imageInterface = imageService;
        _userInterface = userService;
    }

    [ObservableProperty] private string firstName = string.Empty;
    [ObservableProperty] private string lastName = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string phone = string.Empty;
    [ObservableProperty] private string cnpj = string.Empty;
    [ObservableProperty] private string profileImageUrl = "placeholder_profile.png";
    [ObservableProperty] private bool isBusy;

    public bool IsNotBusy => !IsBusy;
    public ObservableCollection<Notification> Notifications { get; set; } = new();
    public bool HasErrors => Notifications.Any();
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public IEnumerable GetErrors(string? propertyName) =>
        Notifications.Where(n => n.Key == propertyName);

    private void ValidateAll()
    {
        try
        {
            Notifications.Clear();

            var contract = new Contract<Notification>()
                .Requires()
                .IsNotNullOrWhiteSpace(FirstName, "Nome", "Nome é obrigatório")
                .IsNotNullOrWhiteSpace(LastName, "Sobrenome", "Sobrenome é obrigatório")
                .IsNotNullOrWhiteSpace(Email, "Email", "E-mail é obrigatório")
                .IsEmail(Email, "Email", "E-mail inválido");

            if (string.IsNullOrWhiteSpace(Cnpj))
                contract.AddNotification("Cnpj", "CNPJ é obrigatório");
            else
                contract.IsCnpj(Cnpj, "Cnpj", "CNPJ inválido");

            if (string.IsNullOrWhiteSpace(Phone))
                contract.AddNotification("Phone", "Telefone é obrigatório");
            else if (!Regex.IsMatch(Phone, @"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$"))
                contract.AddNotification("Phone", "Telefone inválido");

            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                contract.AddNotification("Password", "Senha precisa ter pelo menos 6 caracteres");

            Notifications = new ObservableCollection<Notification>(contract.Notifications);

            foreach (var prop in Notifications.Select(n => n.Key).Distinct())
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(prop));
        }
        catch (Exception ex)
        {
            Application.Current.MainPage?.DisplayAlert("Erro interno", ex.Message, "OK");
        }
    }

    partial void OnProfileImageUrlChanged(string oldValue, string newValue)
    {
        if (string.IsNullOrWhiteSpace(newValue))
            ProfileImageUrl = "placeholder_profile.png";
    }

    [RelayCommand]
    private async Task PickImageAsync()
    {
        var file = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Escolha uma imagem de perfil"
        });

        if (file == null) return;

        var response = await _imageInterface.UploadProfileImageAsync(file);
        if (!string.IsNullOrEmpty(response?.Image))
        {
            ProfileImageUrl = response.Image;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao enviar imagem", "OK");
        }
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            ValidateAll();

            if (HasErrors || string.IsNullOrWhiteSpace(ProfileImageUrl))
            {
                var msg = string.Join("\n", Notifications.Select(n => $"{n.Key}: {n.Message}"));
                if (string.IsNullOrWhiteSpace(ProfileImageUrl))
                    msg += "\nImagem de perfil é obrigatória";

                await Application.Current.MainPage.DisplayAlert("Erro de validação", msg.Trim(), "OK");
                return;
            }

            var dto = new SignUpRequestDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Phone = Phone,
                Cnpj = Cnpj,
                ProfileImageUrl = ProfileImageUrl
            };

            await _userInterface.SignUpUserAsync(dto);

            var loginDto = new SignInRequestDto
            {
                Email = Email,
                Password = Password
            };

            await _userInterface.SignInUserAsync(loginDto);

            var currentUser = await _userInterface.GetCurrentUserAsync();
            UserSession.CurrentUser = currentUser;

            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//MainApp/HomePage");
        }
        catch (Exception ex)
        {
            var error = ex.InnerException?.Message ?? ex.Message;

            // Trata possíveis erros de conflito retornados do backend (409)
            if (error.Contains("já está em uso"))
            {
                await Application.Current.MainPage.DisplayAlert("Erro de cadastro", error, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Erro inesperado: " + error, "OK");
            }
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    [RelayCommand]
    private async Task NavigateToLoginAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }
}
