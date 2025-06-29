using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class ReservationHistoryPage : ContentPage
{
    public ReservationHistoryPage(ReservationHistoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
