using System.Collections.ObjectModel;

namespace IT488_Reg_Form;

public partial class RewardsPage : ContentPage
{
    private readonly ObservableCollection<PointsTransaction> _txns = new();
    private int _balance;

    public RewardsPage()
    {
        InitializeComponent();          // will work once XAML compiles
        TxnView.ItemsSource = _txns;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (string.IsNullOrWhiteSpace(SessionService.CurrentUserEmail))
        {
            await DisplayAlert("Sign in required", "Please sign in to view your rewards.", "OK");
            await Navigation.PopAsync();
            return;
        }

        var email = SessionService.CurrentUserEmail;

        var user = await App.Database.GetByEmailAsync(email);
        if (user is null)
        {
            await DisplayAlert("Not found", "Could not load your profile.", "OK");
            await Navigation.PopAsync();
            return;
        }

        _balance = user.PointsBalance;
        PointsLabel.Text = _balance.ToString();

        // Compute tier + progress
        var (tier, progress) = ComputeTier(_balance);
        TierLabel.Text = tier;
        TierProgress.Progress = progress;

        // Load transactions
        var items = await App.Database.GetTransactionsByEmailAsync(email);
        _txns.Clear();
        foreach (var t in items) _txns.Add(t);
    }

    // Demo thresholds: Bronze <500, Silver 500–1499, Gold 1500–2999, Platinum 3000+
    private (string tier, double progress) ComputeTier(int points)
    {
        if (points >= 3000) return ("Platinum", 1.0);
        if (points >= 1500) return ("Gold", Math.Min(1.0, (points - 1500) / 1500.0));
        if (points >= 500) return ("Silver", Math.Min(1.0, (points - 500) / 1000.0));
        return ("Bronze", Math.Min(1.0, points / 500.0));
    }

    async void Redeem_Clicked(object sender, EventArgs e)
    {
        RedeemError.IsVisible = false;

        if (string.IsNullOrWhiteSpace(SessionService.CurrentUserEmail))
        {
            RedeemError.Text = "Sign in required.";
            RedeemError.IsVisible = true;
            return;
        }

        if (!int.TryParse(RedeemEntry.Text?.Trim(), out var toRedeem) || toRedeem <= 0)
        {
            RedeemError.Text = "Enter a positive number of points.";
            RedeemError.IsVisible = true;
            return;
        }

        var ok = await App.Database.RedeemPointsAsync(SessionService.CurrentUserEmail, toRedeem, "User redemption");
        if (!ok)
        {
            RedeemError.Text = "Not enough points.";
            RedeemError.IsVisible = true;
            return;
        }

        await DisplayAlert("Redeemed", $"Redeemed {toRedeem} points.", "OK");
        OnAppearing(); // reload balance and history
    }
}
