using acheesporte_locator_app.ViewModels;   // namespace onde está a VM
using Microsoft.Maui.Controls;

namespace acheesporte_locator_app.Views
{
    public partial class ReservationListPage : ContentPage
    {
        private readonly ReservationListViewModel _viewModel;

        // A VM é resolvida pelo DI (registrada como Transient ou Scoped no MauiProgram)
        public ReservationListPage(ReservationListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        /// <summary>
        ///   Sempre que a página aparecer, carregamos (ou recarregamos) a lista.
        ///   Se o <c>venueId</c> ainda não tiver sido preenchido (por causa de navegação
        ///   muito rápida), o método na VM já contém a verificação e apenas aborta.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Evita disparos redundantes se já estiver carregando
            if (!_viewModel.IsLoading)
            {
                // Executa como fire-and-forget; exceções já são tratadas internamente
                _ = _viewModel.LoadReservationsAsync();
            }
        }
    }
}
