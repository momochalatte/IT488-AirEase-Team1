using System.Collections.ObjectModel;
using System.Linq;

namespace IT488_Reg_Form;

public partial class PaymentConfirmationPage : ContentPage
{
    public ObservableCollection<FlightOffer> PurchasedItems { get; }
    public decimal TotalPaid { get; }
    public string ConfirmationCode { get; }
    public DateTime PurchasedAt { get; } = DateTime.Now;
    public int ItemCount => PurchasedItems.Count;

    public PaymentConfirmationPage(IEnumerable<FlightOffer> purchased, decimal totalPaid)
    {
        InitializeComponent(); 

        PurchasedItems = new ObservableCollection<FlightOffer>(purchased ?? Enumerable.Empty<FlightOffer>());
        TotalPaid = totalPaid;
        ConfirmationCode = GenerateCode();

        BindingContext = this;
    }



    static string GenerateCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var r = new Random();
        return new string(Enumerable.Range(0, 6).Select(_ => chars[r.Next(chars.Length)]).ToArray());
    }

    async void Done_Clicked(object sender, EventArgs e) => await Navigation.PopToRootAsync();

    async void BackToSearch_Clicked(object sender, EventArgs e)
    {
        // Navigate to your existing flight search page
        await Navigation.PushAsync(new FlightSearchPage());
    }


    
}
