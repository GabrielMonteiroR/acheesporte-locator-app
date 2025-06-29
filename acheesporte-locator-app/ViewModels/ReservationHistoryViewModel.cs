using acheesporte_locator_app.Dtos.Reservations;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_locator_app.ViewModels
{
    [QueryProperty(nameof(VenueId), "venueId")]
    public partial class ReservationHistoryViewModel : ObservableObject
    {
        private readonly IReservationService _reservationService;

        public ReservationHistoryViewModel(IReservationService reservationService)
        {
            _reservationService = reservationService;
            Reservations = new ObservableCollection<ReservationResponseDto>();
        }

        [ObservableProperty] private int venueId;
        [ObservableProperty] private bool isLoading;

        public ObservableCollection<ReservationResponseDto> Reservations { get; }

        public bool IsEmpty => !Reservations.Any();

        partial void OnVenueIdChanged(int value) => _ = LoadAsync();

        [RelayCommand]
        private async Task LoadAsync()
        {
            if (VenueId <= 0) return;

            try
            {
                IsLoading = true;
                Reservations.Clear();

                var history = await _reservationService.GetReservationHistoryByVenueIdAsync(VenueId);
                foreach (var r in history)
                    Reservations.Add(r);

                OnPropertyChanged(nameof(IsEmpty));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar histórico: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private Task RefreshAsync() => LoadAsync();
    }

}
