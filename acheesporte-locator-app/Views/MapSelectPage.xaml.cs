using acheesporte_locator_app.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;

namespace acheesporte_locator_app.Views;

public partial class MapSelectPage : ContentPage
{
    private Location _selectedLocation;

    public MapSelectPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permissão necessária", "Permita o uso da localização para usar o mapa.", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }
            }

            var currentLocation = await Geolocation.GetLastKnownLocationAsync();
            if (currentLocation != null)
            {
                var mapSpan = MapSpan.FromCenterAndRadius(
                    new Microsoft.Maui.Devices.Sensors.Location(currentLocation.Latitude, currentLocation.Longitude),
                    Distance.FromKilometers(1));

                MapControl.MoveToRegion(mapSpan);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar o mapa: {ex.Message}", "OK");
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
            await DisplayAlert("Aviso", "Selecione um local no mapa antes de confirmar.", "OK");
            return;
        }

        WeakReferenceMessenger.Default.Send(new LocationSelectedMessage((_selectedLocation.Latitude, _selectedLocation.Longitude)));
        await Shell.Current.GoToAsync("..");
    }
}
