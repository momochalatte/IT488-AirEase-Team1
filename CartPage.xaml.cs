namespace IT488_Reg_Form;

public partial class CartPage : ContentPage
{
    public CartPage()
    {
        InitializeComponent();
        BindingContext = CartService.Instance;
    }

    void RemoveSwipe_Invoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem si && si.CommandParameter is FlightOffer offer)
            CartService.Instance.Remove(offer);
    }

    async void Checkout_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaymentPage());
    }
}
