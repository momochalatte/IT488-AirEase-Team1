using System.Collections.ObjectModel;

namespace IT488_Reg_Form;

 partial class FlightSearchPage : ContentPage
{
    readonly FlightSearchService _service = new();

    public ObservableCollection<FlightOffer> Offers { get; } = new();

    public FlightSearchPage()
    {
        InitializeComponent();

        // Fill pickers
        PassengersPicker.ItemsSource = Enumerable.Range(1, 9).Select(i => i.ToString()).ToList();
        ClassPicker.ItemsSource = new[] { "Economy", "Premium Economy", "Business", "First" };

        // Defaults like the mock
        OneWayCheck.IsChecked = true;
        PassengersPicker.SelectedIndex = 0;
        ClassPicker.SelectedIndex = 0;

        ResultsView.ItemsSource = Offers;

        // Sensible defaults
        var now = DateTime.Now;
        DepartureDatePicker.Date = now.Date.AddDays(1);
        DepartureTimePicker.Time = new TimeSpan(9, 0, 0);
    }

    void TripType_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        // Make them mutually exclusive, ensure at least one
        if (sender == RoundTripCheck && e.Value) OneWayCheck.IsChecked = false;
        if (sender == OneWayCheck && e.Value) RoundTripCheck.IsChecked = false;
        if (!RoundTripCheck.IsChecked && !OneWayCheck.IsChecked) OneWayCheck.IsChecked = true;
    }

    async void OnSearchFlightsClicked(object? sender, EventArgs e)
    {
        // Quick client-side validation
        if (string.IsNullOrWhiteSpace(DepartingFromEntry.Text) ||
            string.IsNullOrWhiteSpace(DestinationEntry.Text))
        {
            await DisplayAlert("Missing info", "Please enter both Departing From and Destination.", "OK");
            return;
        }
        if (PassengersPicker.SelectedIndex < 0 || ClassPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Missing info", "Please select passenger count and class.", "OK");
            return;
        }

        var criteria = new FlightSearchCriteria
        {
            From = DepartingFromEntry.Text!.Trim(),
            To = DestinationEntry.Text!.Trim(),
            Date = DepartureDatePicker.Date,
            Time = DepartureTimePicker.Time,
            Passengers = int.Parse((string)PassengersPicker.SelectedItem!),
            CabinClass = (string)ClassPicker.SelectedItem!,
            RoundTrip = RoundTripCheck.IsChecked
        };

        // Call mock service, then display cheapest-first
        var results = await _service.SearchAsync(criteria);
        Offers.Clear();
        foreach (var o in results.OrderBy(o => o.Price))
            Offers.Add(o);

        // Flag the cheapest (first after ordering)
        if (Offers.Count > 0) Offers[0].IsCheapest = true;
        ResultsView.ItemsSource = null;  // force refresh to show 'Cheapest' badge
        ResultsView.ItemsSource = Offers;
    }
}
