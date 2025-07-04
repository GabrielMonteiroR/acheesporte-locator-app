﻿
using acheesporte_locator_app.Views;

namespace acheesporte_locator_app;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute("MapSelectPage", typeof(MapSelectPage));
        Routing.RegisterRoute("VenueRegisterPage", typeof(VenueRegisterPage));
        Routing.RegisterRoute("VenueEditPage", typeof(VenueEditPage));
        Routing.RegisterRoute("AvailableTimeListPage", typeof(AvailableTimeListPage));
        Routing.RegisterRoute("AddAvailableTimePage", typeof(AddAvailableTimePage));
        Routing.RegisterRoute("EditAvailableTimePage", typeof(EditAvailableTimePage));
        Routing.RegisterRoute("UserProfilePage", typeof(UserProfilePage));
        Routing.RegisterRoute("ReservationListPage", typeof(ReservationListPage));
        Routing.RegisterRoute(nameof(ReservationHistoryPage), typeof(ReservationHistoryPage));

    }
}