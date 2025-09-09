using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;

namespace IT488_Reg_Form
{
    public partial class CheckInPage : ContentPage
    {
        private ObservableCollection<PassengerItem> _passengers = new();

        public CheckInPage()
        {
            InitializeComponent();

            // Simple airline list. Replace with your real source if needed.
            AirlinePicker.ItemsSource = new[]
            {
                "AirEase", "Delta", "United", "American", "Southwest", "JetBlue", "Alaska"
            };
        }

        // ========== UI EVENTS ==========

        // Fired when you tap "Find Reservation"
        private async void FindReservation_Clicked(object sender, EventArgs e)
        {
            ClearErrors();
            Busy.IsVisible = Busy.IsRunning = true;
            ReservationFrame.IsVisible = false;

            try
            {
                string conf = (ConfirmationEntry.Text ?? "").Trim().ToUpperInvariant();
                string last = (LastNameEntry.Text ?? "").Trim();
                string airline = AirlinePicker.SelectedItem as string ?? "";
                string flight = (FlightNumberEntry.Text ?? "").Trim();
                DateTime date = DepartureDatePicker.Date;

                // Basic validations matching your XAML intent
                if (conf.Length != 6)
                {
                    ConfirmationError.Text = "PNR / confirmation must be exactly 6 characters.";
                    ConfirmationError.IsVisible = true;
                    return;
                }
                if (last.Length < 2)
                {
                    LastNameError.Text = "Enter your last name.";
                    LastNameError.IsVisible = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(airline))
                {
                    AirlineError.Text = "Pick your airline.";
                    AirlineError.IsVisible = true;
                    return;
                }
                if (!int.TryParse(flight, out int flightNo))
                {
                    FlightError.Text = "Flight number must be numeric.";
                    FlightError.IsVisible = true;
                    return;
                }
                if (date.Date < DateTime.Today.AddDays(-1))
                {
                    DateError.Text = "That date looks in the past.";
                    DateError.IsVisible = true;
                    return;
                }

                // Simulate lookup
                await Task.Delay(500);

                // Fill the reservation summary
                ItinLabel.Text = $"{airline} {flightNo} — {conf}";
                WhenLabel.Text = $"{date:MMM d, yyyy}  •  Depart 10:15 AM";

                // Demo passengers; bind to CollectionView
                _passengers = new ObservableCollection<PassengerItem>
                {
                    new PassengerItem { FullName = $"{last}, Alex"   },
                    new PassengerItem { FullName = $"{last}, Taylor" },
                };
                PassengersView.ItemsSource = _passengers;

                ReservationFrame.IsVisible = true;
                UpdateCheckInEnabled();
            }
            finally
            {
                Busy.IsRunning = Busy.IsVisible = false;
            }
        }

        // Fired whenever a passenger checkbox changes inside the CollectionView
        private void Passenger_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // The binding already updates IsSelected on the item;
            // we just need to recompute the button state.
            UpdateCheckInEnabled();
        }

        // Fired when the terms checkbox changes
        private void TermsCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            UpdateCheckInEnabled();
        }

        // Fired when you tap "Check In"
        private async void CheckIn_Clicked(object sender, EventArgs e)
        {
            FormError.IsVisible = false;

            if (!TermsCheck.IsChecked)
            {
                FormError.Text = "Please accept the baggage & check-in terms.";
                FormError.IsVisible = true;
                return;
            }

            if (!_passengers.Any(p => p.IsSelected))
            {
                FormError.Text = "Select at least one passenger to check in.";
                FormError.IsVisible = true;
                return;
            }

            Busy.IsVisible = Busy.IsRunning = true;
            try
            {
                await Task.Delay(600); // simulate network call
                int count = _passengers.Count(p => p.IsSelected);
                await DisplayAlert("Checked in", $"Checked in {count} passenger(s).", "OK");
                await Navigation.PopAsync();
            }
            finally
            {
                Busy.IsRunning = Busy.IsVisible = false;
            }
        }

        // ========== HELPERS ==========

        private void UpdateCheckInEnabled()
        {
            bool anySelected = _passengers.Any(p => p.IsSelected);
            CheckInButton.IsEnabled = anySelected && TermsCheck.IsChecked;
        }

        private void ClearErrors()
        {
            FormError.IsVisible = false;
            ConfirmationError.IsVisible = false;
            LastNameError.IsVisible = false;
            AirlineError.IsVisible = false;
            FlightError.IsVisible = false;
            DateError.IsVisible = false;
        }
    }

    // Simple item for the passenger list
    public class PassengerItem : INotifyPropertyChanged
    {
        private string _fullName = "";
        private bool _isSelected;

        public string FullName
        {
            get => _fullName;
            set { if (_fullName != value) { _fullName = value; OnPropertyChanged(); } }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { if (_isSelected != value) { _isSelected = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
