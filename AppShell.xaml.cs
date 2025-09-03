using AirEase.Pages;

namespace AirEase;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Route for navigation by name:
        Routing.RegisterRoute(nameof(CheckInPage), typeof(CheckInPage));

        // Optional: a shell item so it shows in the tab bar or flyout
        Items.Add(new ShellContent
        {
            Title = "Check-In",
            ContentTemplate = new DataTemplate(typeof(CheckInPage)),
            Route = "checkin"
        });
    }
}
