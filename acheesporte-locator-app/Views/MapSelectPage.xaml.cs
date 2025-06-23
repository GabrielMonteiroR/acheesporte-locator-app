using acheesporte_athlete_app.Dtos.GooglePlaces;
using acheesporte_locator_app.Config;
using acheesporte_locator_app.Messages;
using acheesporte_locator_app.Services;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace acheesporte_locator_app.Views;

public partial class MapSelectPage : ContentPage
{
    private readonly GooglePlacesService _placesService;
    private Location _selectedLocation;

    public MapSelectPage()
    {
        InitializeComponent();

        var apiSettings = App.Current.Handler.MauiContext.Services.GetService<ApiSettings>();
        _placesService = new GooglePlacesService(new HttpClient(), apiSettings!);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permissão", "Localização necessária", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            var currentLocation = await Geolocation.GetLastKnownLocationAsync();
            if (currentLocation != null)
            {
                var mapSpan = MapSpan.FromCenterAndRadius(
                    new Location(currentLocation.Latitude, currentLocation.Longitude),
                    Distance.FromKilometers(1));
                MapControl.MoveToRegion(mapSpan);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao carregar localização: {ex.Message}", "OK");
        }
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        _selectedLocation = new Location(e.Location.Latitude, e.Location.Longitude);

        MapControl.Pins.Clear();
        MapControl.Pins.Add(new Pin
        {
            Label = "Local selecionado",
            Location = _selectedLocation,
            Type = PinType.Place
        });
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        if (_selectedLocation is null)
        {
            await DisplayAlert("Aviso", "Selecione um local primeiro.", "OK");
            return;
        }

        WeakReferenceMessenger.Default.Send(new LocationSelectedMessage((_selectedLocation.Latitude, _selectedLocation.Longitude)));
        await Shell.Current.GoToAsync("..");
    }
}
