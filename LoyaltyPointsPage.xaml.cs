namespace AirEase;

public partial class LoyaltyPointsPage : ContentPage
{
    private int _points = 0;

    public LoyaltyPointsPage()
    {
        InitializeComponent();

        BtnEarnPoints.Clicked += (s, e) =>
        {
            _points += 50;
            UpdatePoints();
            DisplayAlert("Points Earned", "You earned 50 points!", "OK");
        };

        BtnRedeemPoints.Clicked += (s, e) =>
        {
            if (_points >= 100)
            {
                _points -= 100;
                UpdatePoints();
                DisplayAlert("Redeemed", "You redeemed 100 points!", "OK");
            }
            else
            {
                DisplayAlert("Not Enough Points", "You need at least 100 points to redeem.", "OK");
            }
        };
    }

    private void UpdatePoints()
    {
        PointsValue.Text = $"{_points} Points";
    }
}
