using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IT488_Reg_Form;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Load remembered email if stored
        var savedEmail = await SecureStorage.GetAsync("saved_email");
        if (!string.IsNullOrWhiteSpace(savedEmail))
        {
            EmailEntry.Text = savedEmail;
            RememberMeCheck.IsChecked = true;
        }

        EmailEntry.Focus();
        RecomputeFormValidity();
    }

    // Toggle password visibility
    void ShowPwd_CheckedChanged(object sender, CheckedChangedEventArgs e)
        => PasswordEntry.IsPassword = !e.Value;

    // Basic client-side validation gate
    void AnyField_Changed(object sender, TextChangedEventArgs e) => RecomputeFormValidity();

    void RecomputeFormValidity()
    {
        AuthError.IsVisible = false; 

        bool emailOk = IsValidEmail(EmailEntry.Text);
        bool pwdOk = !string.IsNullOrWhiteSpace(PasswordEntry.Text) && PasswordEntry.Text.Length >= 8;

        EmailError.IsVisible = !emailOk;
        EmailError.Text = emailOk ? "" : "Enter a valid email address.";

        SignInButton.IsEnabled = emailOk && pwdOk;
    }

    static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    async void SignInButton_Clicked(object sender, EventArgs e)
    {
        RecomputeFormValidity();
        if (!SignInButton.IsEnabled) return;

        BusyIndicator.IsVisible = true;
        BusyIndicator.IsRunning = true;
        SignInButton.IsEnabled = true;
        AuthError.IsVisible = false;

        try
        {
            var email = EmailEntry.Text!.Trim().ToLowerInvariant();
            var pwd = PasswordEntry.Text!;

            var profile = await App.Database.GetByEmailAsync(email);
            if (profile is null)
            {
                AuthError.Text = "No account matches that email.";
                AuthError.IsVisible = true;
                return;
            }

            bool ok = false;

          
            if (!string.IsNullOrEmpty(profile.PasswordHash) && profile.PasswordHash.StartsWith("$2"))
            {
                ok = BCrypt.Net.BCrypt.Verify(pwd, profile.PasswordHash);
            }
            else
            {
                
                ok = string.Equals(profile.PasswordHash ?? "", pwd, StringComparison.Ordinal);
                if (ok)
                {
                    profile.PasswordHash = BCrypt.Net.BCrypt.HashPassword(pwd);
                    await App.Database.UpdateAsync(profile); 
                }
            }

            if (!ok)
            {
                AuthError.Text = "Incorrect password. Please try again.";
                AuthError.IsVisible = true;
                return;
            }

            if (RememberMeCheck.IsChecked) await SecureStorage.SetAsync("saved_email", email);
            else SecureStorage.Remove("saved_email");

            SessionService.CurrentUserEmail = profile.Email;

            
            // await DisplayAlert("OK", "Signed in. Navigating…", "OK");

            if (Application.Current?.MainPage is NavigationPage nav)
            {
                await nav.PushAsync(new ProfilePage(profile));
            }
            else
            {
                
                Application.Current!.MainPage = new NavigationPage(new ProfilePage(profile));
            }
        }
        catch (Exception navEx)
        {
            
            await DisplayAlert("Navigation error", navEx.Message, "OK");
        }
        finally
        {
            BusyIndicator.IsVisible = false;
            BusyIndicator.IsRunning = false;
            RecomputeFormValidity();
        }
        await DisplayAlert("Tapped", "Sign In clicked.", "OK");


   

    }

    



    // Register button -> navigate to your registration form
    async void Register_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new IT488_Reg_Form()); // your existing registration page
    }

    // Forgot password button 
    async void Forgot_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AccountRecoveryPage());
    }
  
}
