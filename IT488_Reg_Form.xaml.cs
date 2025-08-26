using System;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;

namespace IT488_Reg_Form;

partial class IT488_Reg_Form : ContentPage
{
    const int MinAgeYears = 18;

    // Change the constructor name from 'MainPage' to match the class name 'IT488_Reg_Form'
    public IT488_Reg_Form()
    {
        InitializeComponent();

        // Sensible DOB limits
        DobPicker.MaximumDate = DateTime.Today.AddYears(-MinAgeYears);
        DobPicker.MinimumDate = DateTime.Today.AddYears(-120); // unlikely to scroll forever :)

        // Placeholder profile image (optional: add Resources/Images/profile_placeholder.png)
        ProfileImage.Source = "profile_placeholder.png";


        // <-- This is the handler for:  <Button Text="Go to Flight Search" Clicked="GoSearch_Clicked"/>
        async void GoSearch_Clicked(object sender, EventArgs e)
        {
            // Navigate to the flight search page
            await Navigation.PushAsync(new FlightSearchPage());
        }
    }

    async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        PhotoError.IsVisible = false;

        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a profile picture"
            });

            if (result is null)
                return;

            // Copy to cache and display
            var localPath = Path.Combine(FileSystem.CacheDirectory, result.FileName);

            using (var sourceStream = await result.OpenReadAsync())
            using (var dest = File.Open(localPath, FileMode.Create, FileAccess.Write))
            {
                await sourceStream.CopyToAsync(dest);
            }

            ProfileImage.Source = ImageSource.FromFile(localPath);
        }
        catch (Exception ex)
        {
            PhotoError.Text = $"Couldn’t pick photo: {ex.Message}";
            PhotoError.IsVisible = true;
        }

        RecomputeFormValidity();
    }

    void DobPicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        ValidateDob();
        RecomputeFormValidity();
    }

    void AnyField_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordMatch();
        RecomputeFormValidity();
    }

    void ValidateDob()
    {
        var age = GetAgeInYears(DobPicker.Date, DateTime.Today);
        if (age < MinAgeYears)
        {
            DobError.Text = $"You must be at least {MinAgeYears} years old.";
            DobError.IsVisible = true;
        }
        else
        {
            DobError.IsVisible = false;
        }
    }

    void ValidatePasswordMatch()
    {
        if (string.IsNullOrEmpty(PasswordEntry.Text) && string.IsNullOrEmpty(ConfirmPasswordEntry.Text))
        {
            ConfirmError.IsVisible = false;
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            ConfirmError.Text = "Passwords do not match.";
            ConfirmError.IsVisible = true;
        }
        else
        {
            ConfirmError.IsVisible = false;
        }
    }

    static int GetAgeInYears(DateTime dob, DateTime now)
    {
        int age = now.Year - dob.Year;
        if (dob.Date > now.AddYears(-age)) age--;
        return age;
    }

    void RecomputeFormValidity()
    {
        // Check toolkit behaviors via attached IsValid flags (Entry.TextColor turns green/red, but we gate on content too)
        bool firstOk = !string.IsNullOrWhiteSpace(FirstNameEntry.Text);
        bool lastOk = !string.IsNullOrWhiteSpace(LastNameEntry.Text);
        bool addrOk = !string.IsNullOrWhiteSpace(AddressEntry.Text);
        bool emailOk = !string.IsNullOrWhiteSpace(EmailEntry.Text);
        bool phoneOk = !string.IsNullOrWhiteSpace(PhoneEntry.Text);

        bool dobOk = !DobError.IsVisible && DobPicker.Date != default;

        bool pwdOk = !string.IsNullOrWhiteSpace(PasswordEntry.Text);
        bool confirmOk = !ConfirmError.IsVisible && !string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text);

        // Simple final gate; toolkit already enforces formats/lengths on change
        SignUpButton.IsEnabled = firstOk && lastOk && addrOk && emailOk && phoneOk && dobOk && pwdOk && confirmOk;
    }

    async void SignUpButton_Clicked(object sender, EventArgs e)
    {
        // Final server-side-like checks (defense in depth)
        ValidateDob();
        ValidatePasswordMatch();
        RecomputeFormValidity();

        if (!SignUpButton.IsEnabled)
        {
            await DisplayAlert("Check form", "Please correct the highlighted fields.", "OK");
            return;
        }

        // TODO: send to your backend
        await DisplayAlert("Welcome to AirEase!", "Your profile has been created.", "OK");
    }

     async void GoSearch_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FlightSearchPage());
       

    }

}
