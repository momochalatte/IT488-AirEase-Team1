using Microsoft.Maui.Controls;
using NPOI.SS.Formula.Functions;

namespace PaymentPage
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnBookNowClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                string.IsNullOrWhiteSpace(CardNumberEntry.Text) ||
                string.IsNullOrWhiteSpace(ExpiryDateEntry.Text) ||
                string.IsNullOrWhiteSpace(cvvEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            await DisplayAlert("Success", "Your booking has been confirmed!", "OK");
        }

        private void OnPayNowClicked(object sender, EventArgs e)
        {
        }
    }
}
