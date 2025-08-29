
using PaymentPage;
using Microsoft.Maui.Controls;
using Android.Provider;


namespace PaymentPage
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
#if WINDOWS || ANDROID || IOS || MACCATALYST
            this.LoadFromXaml(typeof(AppShell));
#else
                InitializeComponent();
#endif
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(BookingPage), typeof(BookingPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(Settings), typeof(Settings));    
        }
    }
}
