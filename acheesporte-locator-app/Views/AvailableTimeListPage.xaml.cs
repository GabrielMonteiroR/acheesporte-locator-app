using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class AvailableTimeListPage : ContentPage
{
    private readonly AvailableTimeListViewModel _viewModel;

    public AvailableTimeListPage(AvailableTimeListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.VenueId > 0)
        {
            await _viewModel.LoadAvailabilityTimesCommand.ExecuteAsync(null);
        }
    }
}
