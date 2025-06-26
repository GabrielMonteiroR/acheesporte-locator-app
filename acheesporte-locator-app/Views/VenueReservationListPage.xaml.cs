using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class VenueReservationListPage : ContentPage
{
    private readonly VenueViewModel _viewModel;

    public VenueReservationListPage(VenueViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadVenuesAsync();
    }
}
