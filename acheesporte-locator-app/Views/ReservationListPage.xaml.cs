using acheesporte_locator_app.ViewModels;   // namespace onde est� a VM
using Microsoft.Maui.Controls;

namespace acheesporte_locator_app.Views
{
    public partial class ReservationListPage : ContentPage
    {
        private readonly ReservationListViewModel _viewModel;

        // A VM � resolvida pelo DI (registrada como Transient ou Scoped no MauiProgram)
        public ReservationListPage(ReservationListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        /// <summary>
        ///   Sempre que a p�gina aparecer, carregamos (ou recarregamos) a lista.
        ///   Se o <c>venueId</c> ainda n�o tiver sido preenchido (por causa de navega��o
        ///   muito r�pida), o m�todo na VM j� cont�m a verifica��o e apenas aborta.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Evita disparos redundantes se j� estiver carregando
            if (!_viewModel.IsLoading)
            {
                // Executa como fire-and-forget; exce��es j� s�o tratadas internamente
                _ = _viewModel.LoadReservationsAsync();
            }
        }
    }
}
