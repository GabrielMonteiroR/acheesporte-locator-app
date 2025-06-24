using acheesporte_locator_app.Dtos.Availability;
using acheesporte_locator_app.Interfaces;
using acheesporte_locator_app.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

[QueryProperty(nameof(VenueId), "venueId")]
public partial class AvailableTimeListViewModel : ObservableObject
{
    private readonly IAvailableTimesService _availableTimesService;

    public AvailableTimeListViewModel(AvailableTimesService availableTimesService)
    {
        _availableTimesService = availableTimesService;
        AvailabilityTimes = new ObservableCollection<VenueAvailabilityTimeDto>();
    }

    [ObservableProperty]
    private int venueId;

    [ObservableProperty]
    private bool isLoading;

    public ObservableCollection<VenueAvailabilityTimeDto> AvailabilityTimes { get; }

    public bool IsEmpty => !AvailabilityTimes.Any();

    [RelayCommand]
    public async Task LoadAvailabilityTimesAsync()
    {
        try
        {
            IsLoading = true;
            AvailabilityTimes.Clear();

            var items = await _availableTimesService.GetAvailableTimesByVenueIdAsync(VenueId, isReserved: false);

            foreach (var item in items)
                AvailabilityTimes.Add(item);

            OnPropertyChanged(nameof(IsEmpty));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar horários: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public void EditAvailabilityTime(VenueAvailabilityTimeDto time)
    {
        // Por enquanto, apenas visual.
        Console.WriteLine($"Editar horário ID: {time.Id}");
    }

    [RelayCommand]
    public void DeleteAvailabilityTime(VenueAvailabilityTimeDto time)
    {
        // Por enquanto, apenas visual.
        Console.WriteLine($"Apagar horário ID: {time.Id}");
    }
}
