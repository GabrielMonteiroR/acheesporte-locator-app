using System.Collections.ObjectModel;
using acheesporte_locator_app.Dtos.Reservations;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace acheesporte_locator_app.ViewModels;

[QueryProperty(nameof(VenueId), "venueId")]
public partial class ReservationListViewModel : ObservableObject
{
    private readonly IReservationService _reservationService;

    public ReservationListViewModel(IReservationService reservationService)
    {
        _reservationService = reservationService;
        Reservations = new ObservableCollection<ReservationResponseDto>();
    }

    [ObservableProperty] private int venueId;
    [ObservableProperty] private bool isLoading;

    public ObservableCollection<ReservationResponseDto> Reservations { get; }

    public bool IsEmpty => !Reservations.Any();

    partial void OnVenueIdChanged(int value) => _ = LoadReservationsAsync();

    [RelayCommand]
    public async Task LoadReservationsAsync()
    {
        if (VenueId <= 0) return;

        try
        {
            IsLoading = true;
            Reservations.Clear();

            var list = await _reservationService.GetReservationsByVenueIdAsync(VenueId);

            foreach (var res in list)
                Reservations.Add(res);

            OnPropertyChanged(nameof(IsEmpty));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro",
                $"Não foi possível carregar as reservas.\n{ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand] private Task RefreshAsync() => LoadReservationsAsync();


}
