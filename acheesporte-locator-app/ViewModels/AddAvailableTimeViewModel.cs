using acheesporte_locator_app.Dtos.AvailabilityTimes;
using acheesporte_locator_app.Helpers;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

[QueryProperty(nameof(VenueId), "venueId")]
public partial class AddAvailableTimeViewModel : ObservableObject
{
    private readonly IAvailableTimesService _availableTimesService;

    public AddAvailableTimeViewModel(IAvailableTimesService availableTimesService)
    {
        _availableTimesService = availableTimesService;
        ReservedTimes = new ObservableCollection<VenueAvailabilityTimeDto>();
    }

    [ObservableProperty]
    private int venueId;

    partial void OnVenueIdChanged(int value)
    {
        _ = LoadReservedTimesAsync();
    }

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private TimeSpan selectedStartTime;

    [ObservableProperty]
    private TimeSpan selectedEndTime;

    [ObservableProperty]
    private decimal price;

    [ObservableProperty]
    private bool isLoading;

    public ObservableCollection<VenueAvailabilityTimeDto> ReservedTimes { get; }

    [RelayCommand]
    public async Task LoadReservedTimesAsync()
    {
        try
        {
            IsLoading = true;
            ReservedTimes.Clear();

            var list = await _availableTimesService.GetAvailableTimesByVenueIdAsync(VenueId);

            foreach (var item in list)
                ReservedTimes.Add(item);

            var timesSummary = string.Join("\n", list.Select(t => $"Start: {t.StartDate}, End: {t.EndDate}, Price: {t.Price}"));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar horários existentes: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }


    [RelayCommand]
    public async Task CreateAvailabilityAsync()
    {
        try
        {
            var startDateTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                                             SelectedStartTime.Hours, SelectedStartTime.Minutes, 0, DateTimeKind.Utc);

            var endDateTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                                           SelectedEndTime.Hours, SelectedEndTime.Minutes, 0, DateTimeKind.Utc);

            if (startDateTime >= endDateTime)
            {
                await Shell.Current.DisplayAlert("Erro", "O horário inicial deve ser antes do final.", "OK");
                return;
            }

            if (ReservedTimes.Any(r => startDateTime < r.EndDate && endDateTime > r.StartDate))
            {
                await Shell.Current.DisplayAlert("Conflito", "Este horário já está reservado ou sobrepõe outro.", "OK");
                return;
            }

            var dto = new CreateVenueAvailabilityTimeDto
            {
                StartDate = startDateTime,
                EndDate = endDateTime,
                VenueId = VenueId,
                Price = Price,
                IsReserved = false,
                UserId = null
            };

            var response = await _availableTimesService.CreateAvailabilityTimeAsync(dto);

            await Shell.Current.DisplayAlert("Sucesso", "Horário criado com sucesso!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
    }

}
