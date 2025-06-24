using acheesporte_locator_app.Dtos.VenueDtos;
using acheesporte_locator_app.Helpers;
using acheesporte_locator_app.Services;
using acheesporte_locator_app.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

public partial class VenueViewModel : ObservableObject
{
    private readonly VenueService _venueService;

    public VenueViewModel(VenueService venueService)
    {
        _venueService = venueService;
        Venues = new ObservableCollection<VenueResponseDto>();
    }

    [ObservableProperty]
    private bool isLoading;

    public ObservableCollection<VenueResponseDto> Venues { get; }

    public bool IsEmpty => !Venues.Any();

    [RelayCommand]
    public async Task LoadVenuesAsync()
    {
        try
        {
            IsLoading = true;
            Venues.Clear();

            if (UserSession.CurrentUser == null)
                throw new Exception("Usuário não está carregado na sessão.");

            var venues = await _venueService.GetVenuesByOwnerAsync();
            foreach (var venue in venues)
                Venues.Add(venue);

            OnPropertyChanged(nameof(IsEmpty));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar locais: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task NavigateToVenueRegisterAsync()
    {
        await Shell.Current.GoToAsync("VenueRegisterPage");
    }

    [RelayCommand]
    public async Task NavigateToEditVenueAsync(VenueResponseDto venue)
    {
        if (venue == null) return;

        await Shell.Current.GoToAsync(nameof(VenueEditPage), true, new Dictionary<string, object>
    {
        { "venueId", venue.Id }
    });
    }

    [RelayCommand]
    public async Task NavigateToAvailableTimeListAsync(VenueResponseDto venue)
    {
        if (venue == null) return;

        await Shell.Current.GoToAsync("AvailableTimeListPage", true, new Dictionary<string, object>
    {
        { "venueId", venue.Id }
    });
    }


}
