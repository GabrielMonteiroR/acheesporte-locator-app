// Views/EditAvailableTimePage.xaml.cs
using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class EditAvailableTimePage : ContentPage
{
    private readonly EditAvailableTimeViewModel _vm;

    public EditAvailableTimePage(EditAvailableTimeViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_vm.TimeId == 0 || _vm.VenueId == 0)
        {
        }
    }
}
