using acheesporte_locator_app.Dtos.AvailabilityTimes;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

[QueryProperty(nameof(VenueId), "venueId")]
public partial class AvailableTimeListViewModel : ObservableObject
{
    private readonly IAvailableTimesService _availableTimesService;

    public AvailableTimeListViewModel(IAvailableTimesService availableTimesService)
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
    public async Task EditAvailabilityTimeAsync(VenueAvailabilityTimeDto time)
    {
        await Shell.Current.GoToAsync("EditAvailableTimePage", new Dictionary<string, object>
        {
            ["timeId"] = time.Id,
            ["venueId"] = VenueId
        });
    }



    [RelayCommand]
    public async Task DeleteAvailabilityTimeAsync(VenueAvailabilityTimeDto time)
    {
        var confirm = await Shell.Current.DisplayAlert("Confirmação", $"Deseja excluir o horário das {time.StartDate:t} às {time.EndDate:t}?", "Sim", "Não");
        if (!confirm) return;

        try
        {
            var result = await _availableTimesService.DeleteAvailabilityTimeAsync(time.Id);
            if (result)
            {
                AvailabilityTimes.Remove(time);
                await Shell.Current.DisplayAlert("Sucesso", "Horário excluído com sucesso!", "OK");
                OnPropertyChanged(nameof(IsEmpty));
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao excluir horário: {ex.Message}", "OK");
        }
    }


    [RelayCommand]
    public async Task NavigateToAddAvailableTimeAsync()
    {
        await Shell.Current.GoToAsync("AddAvailableTimePage", true, new Dictionary<string, object>
    {
        { "venueId", VenueId }
    });
    }

}
