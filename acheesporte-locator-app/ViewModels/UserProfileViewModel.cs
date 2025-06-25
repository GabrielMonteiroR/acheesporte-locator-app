using acheesporte_locator_app.Dtos.Users;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace acheesporte_locator_app.ViewModels;

public partial class UserProfileViewModel : ObservableObject
{
    private readonly IUserService _userService;

    public UserProfileViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string phone;

    [ObservableProperty]
    private string profileImageUrl;

    [ObservableProperty]
    private bool isLoading;

    [RelayCommand]
    public async Task LoadUserAsync()
    {
        try
        {
            IsLoading = true;

            var currentUser = await _userService.GetCurrentUserAsync();
            UserId = currentUser.Id;

            var user = await _userService.GetUserByIdAsync(UserId);

            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            ProfileImageUrl = user.ProfileImageUrl;

            await Shell.Current.DisplayAlert("Usuário", $"Bem-vindo, {user.FirstName}", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar dados do usuário: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }


    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            IsLoading = true;

            var updateDto = new UpdateUserRequestDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone
            };

            var updatedUser = await _userService.UpdateUserAsync(UserId, updateDto);

            await Shell.Current.DisplayAlert("Sucesso", "Dados atualizados com sucesso!", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao atualizar dados: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
