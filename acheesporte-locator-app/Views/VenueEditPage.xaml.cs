using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class VenueEditPage : ContentPage
{
    private readonly VenueEditViewModel _viewModel;

    public VenueEditPage(VenueEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}
