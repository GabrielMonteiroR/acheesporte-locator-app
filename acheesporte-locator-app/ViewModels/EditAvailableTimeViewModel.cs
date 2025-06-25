using acheesporte_locator_app.Dtos.AvailabilityTimes;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

[QueryProperty(nameof(TimeId), "timeId")]
[QueryProperty(nameof(VenueId), "venueId")]
public partial class EditAvailableTimeViewModel : ObservableObject
{
    private readonly IAvailableTimesService _service;

    public EditAvailableTimeViewModel(IAvailableTimesService service)
    {
        _service = service;
        AllTimesForValidation = new ObservableCollection<VenueAvailabilityTimeDto>();
    }

    [ObservableProperty] private int timeId;
    [ObservableProperty] private int venueId;

    partial void OnTimeIdChanged(int value) => _ = LoadAsync();
    partial void OnVenueIdChanged(int value) => _ = LoadAsync();

    [ObservableProperty] private DateTime selectedDate;
    [ObservableProperty] private TimeSpan selectedStartTime;
    [ObservableProperty] private TimeSpan selectedEndTime;
    [ObservableProperty] private decimal price;
    [ObservableProperty] private bool isLoading;

    public ObservableCollection<VenueAvailabilityTimeDto> AllTimesForValidation { get; }

    private async Task LoadAsync()
    {
        if (TimeId == 0 || VenueId == 0) return;

        try
        {
            IsLoading = true;

            var current = await _service.GetAvailabilityTimeByIdAsync(TimeId);
            var all = await _service.GetAvailableTimesByVenueIdAsync(VenueId);

            SelectedDate = current.StartDate.Date;
            SelectedStartTime = current.StartDate.TimeOfDay;
            SelectedEndTime = current.EndDate.TimeOfDay;
            Price = current.Price;

            AllTimesForValidation.Clear();
            foreach (var t in all.Where(t => t.Id != TimeId))
                AllTimesForValidation.Add(t);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var start = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                                 SelectedStartTime.Hours, SelectedStartTime.Minutes, 0, DateTimeKind.Utc);

        var end = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day,
                                 SelectedEndTime.Hours, SelectedEndTime.Minutes, 0, DateTimeKind.Utc);

        if (start >= end)
        {
            await Shell.Current.DisplayAlert("Erro", "Início deve ser antes do fim.", "OK");
            return;
        }

        if (AllTimesForValidation.Any(t => start < t.EndDate && end > t.StartDate))
        {
            await Shell.Current.DisplayAlert("Conflito",
                "Horário se sobrepõe a outro já existente.", "OK");
            return;
        }

        var dto = new UpdateVenueAvailabilityTimeDto
        {
            StartDate = start,
            EndDate = end,
            Price = Price
        };

        try
        {
            await _service.UpdateAvailabilityTimeAsync(TimeId, dto);
            await Shell.Current.DisplayAlert("Sucesso", "Horário atualizado!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}
