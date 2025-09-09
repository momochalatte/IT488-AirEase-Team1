using System.Text.RegularExpressions;
using System.Linq; 

namespace IT488_Reg_Form;

public partial class PaymentPage : ContentPage
{
    public PaymentPage()
    {
        InitializeComponent();
        BindingContext = CartService.Instance; 
    }

    async void PayNow_Clicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var name = (NameOnCardEntry.Text ?? "").Trim();
        var number = (CardNumberEntry.Text ?? "").Replace(" ", "");
        var expiry = (ExpiryEntry.Text ?? "").Trim();
        var cvv = (CvvEntry.Text ?? "").Trim();

        // Basic client-side validations
        if (name.Length < 2) { ShowError("Enter the name on the card."); return; }
        if (!Regex.IsMatch(number, @"^\d{13,19}$")) { ShowError("Card number must be 13–19 digits."); return; }
        if (!Regex.IsMatch(expiry, @"^(0[1-9]|1[0-2])\/?\d{2}$")) { ShowError("Expiry must be MM/YY."); return; }
        if (!Regex.IsMatch(cvv, @"^\d{3,4}$")) { ShowError("CVV must be 3–4 digits."); return; }

        if (CartService.Instance.Items.Count == 0)
        {
            await DisplayAlert("Cart", "Your cart is empty.", "OK");
            return;
        }

        // 1) Take a snapshot BEFORE clearing (so confirmation has data)
        var snapshot = CartService.Instance.Items.ToList();
        var total = CartService.Instance.Subtotal;

        // 2) Simulate processing
        await Task.Delay(600);

        // ==== LOYALTY: award points (1 point per $1) ====
        if (!string.IsNullOrWhiteSpace(SessionService.CurrentUserEmail))
        {
            // e.g., 1 point per dollar, rounded down
            int points = (int)Math.Floor(total);
            await App.Database.AddPointsAsync(SessionService.CurrentUserEmail, points, "Flight purchase");
        }

        // 3) Clear the cart AFTER processing
        CartService.Instance.Clear();

        // 4) Navigate to confirmation page (do NOT PopToRoot here)
        await Navigation.PushAsync(new PaymentConfirmationPage(snapshot, total));
    }

    void ShowError(string msg)
    {
        ErrorLabel.Text = msg;
        ErrorLabel.IsVisible = true;
    }
}
