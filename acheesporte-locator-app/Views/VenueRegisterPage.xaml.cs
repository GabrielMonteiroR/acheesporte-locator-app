using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class VenueRegisterPage : ContentPage
{
    private readonly VenueFormViewModel _viewModel;

    public VenueRegisterPage(VenueFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadVenueTypesAsync();
    }

    private void OnPostalCodeChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue) && e.NewTextValue.Length == 8)
        {
            if (_viewModel?.FindAddressByPostalCodeCommand?.CanExecute(null) == true)
            {
                _viewModel.FindAddressByPostalCodeCommand.Execute(null);
            }
        }
    }

}
