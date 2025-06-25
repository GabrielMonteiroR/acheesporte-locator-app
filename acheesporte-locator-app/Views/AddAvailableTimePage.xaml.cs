using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class AddAvailableTimePage : ContentPage
{
    private readonly AddAvailableTimeViewModel _viewModel;

    public AddAvailableTimePage(AddAvailableTimeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.VenueId > 0)
        {
            await _viewModel.LoadReservedTimesCommand.ExecuteAsync(null);
        }
    }
}
