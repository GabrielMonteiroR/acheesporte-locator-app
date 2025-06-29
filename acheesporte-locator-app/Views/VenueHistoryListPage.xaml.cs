using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class VenueHistoryListPage : ContentPage
{
    private readonly VenueViewModel _viewModel;

    public VenueHistoryListPage(VenueViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadVenuesAsync();
    }
}

