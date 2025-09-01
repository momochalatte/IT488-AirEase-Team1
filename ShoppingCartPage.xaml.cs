using System.Collections.ObjectModel;

namespace AirlineShoppingCart;

public partial class ShoppingCartPage : ContentPage
{
    public ObservableCollection<CartItem> CartItems { get; set; }

    public ShoppingCartPage()
    {
        InitializeComponent();

        CartItems = new ObservableCollection<CartItem>
        {
            new CartItem { FlightName="Flight 101", Route="NYC → LAX", Price=299.99, Image="plane.png" },
            new CartItem { FlightName="Flight 202", Route="LAX → ORD", Price=199.99, Image="plane.png" }
        };

        CartItemsCollection.ItemsSource = CartItems;
        UpdateTotal();
    }

    private void RemoveItem_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.CommandParameter as CartItem;

        if (item != null && CartItems.Contains(item))
        {
            CartItems.Remove(item);
            UpdateTotal();
        }
    }

    private void Checkout_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Checkout", "Proceeding to payment...", "OK");
    }

    private void UpdateTotal()
    {
        double total = 0;
        foreach (var item in CartItems)
        {
            total += item.Price;
        }
        TotalPriceLabel.Text = $"Total: ${total:F2}";
    }
}

public class CartItem
{
    public string FlightName { get; set; }
    public string Route { get; set; }
    public double Price { get; set; }
    public string Image { get; set; }
}
