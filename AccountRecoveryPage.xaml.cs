using System.Text.RegularExpressions;
using System.Linq; // for .Any()

namespace IT488_Reg_Form;

public partial class AccountRecoveryPage : ContentPage
{
    public AccountRecoveryPage()
    {
        InitializeComponent();
    }

    // ---------- Forgot Password ----------
    async void ResetPassword_Clicked(object sender, EventArgs e)
    {
        ResetStatus.IsVisible = false;

        var email = ResetEmailEntry.Text?.Trim().ToLowerInvariant();
        var dob = ResetDobPicker.Date;
        var pwd = NewPasswordEntry.Text ?? "";
        var cpwd = ConfirmNewPasswordEntry.Text ?? "";

        if (!IsValidEmail(email))
        {
            ShowResetError("Enter a valid email address.");
            return;
        }

        var profile = await App.Database.GetByEmailAsync(email!);
        if (profile is null)
        {
            ShowResetError("No account found with that email.");
            return;
        }

        if (profile.DateOfBirth.Date != dob.Date)
        {
            ShowResetError("Date of birth does not match our records.");
            return;
        }

        var pwdError = ValidatePassword(pwd, cpwd);
        if (pwdError is not null)
        {
            ShowResetError(pwdError);
            return;
        }

        // Update hash and save
        profile.PasswordHash = BCrypt.Net.BCrypt.HashPassword(pwd);
        await App.Database.UpdateAsync(profile);

        await DisplayAlert("Password reset", "Your password has been updated. You can sign in now.", "OK");
        await Navigation.PopAsync(); // go back to Sign In
    }

    // ---------- Forgot Username (email) ----------
    async void FindEmail_Clicked(object sender, EventArgs e)
    {
        FindStatus.IsVisible = false;

        var first = (FindFirstNameEntry.Text ?? "").Trim();
        var last = (FindLastNameEntry.Text ?? "").Trim();
        var dob = FindDobPicker.Date;

        if (first.Length < 2 || last.Length < 2)
        {
            ShowFindError("Enter your first and last name.");
            return;
        }

        // SINGLE result expected
        var prof = await App.Database.FindByNameDobAsync(first, last, dob);
        if (prof is null)
        {
            ShowFindError("No account found. Check your info.");
            return;
        }

        // Show the found email (masked for safety)
        var masked = MaskEmail(prof.Email);
        await DisplayAlert("Account found", $"Email on file: {masked}", "OK");
    }

    // ---------- Helpers ----------
    static bool IsValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) &&
        Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    static string? ValidatePassword(string pwd, string confirm)
    {
        if (string.IsNullOrWhiteSpace(pwd) || pwd.Length < 8) return "Password must be at least 8 characters.";
        if (!pwd.Any(char.IsUpper)) return "Include at least one uppercase letter.";
        if (!pwd.Any(char.IsDigit)) return "Include at least one digit.";
        if (!pwd.Any(c => !char.IsLetterOrDigit(c))) return "Include at least one symbol.";
        if (pwd != confirm) return "Passwords do not match.";
        return null;
    }

    static string MaskEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@')) return email;
        var parts = email.Split('@');
        var local = parts[0];
        if (local.Length <= 1) return "*" + "@" + parts[1];
        return local[0] + new string('*', Math.Max(1, local.Length - 1)) + "@" + parts[1];
    }

    void ShowResetError(string msg)
    {
        ResetStatus.Text = msg;
        ResetStatus.IsVisible = true;
    }

    void ShowFindError(string msg)
    {
        FindStatus.Text = msg;
        FindStatus.IsVisible = true;
    }
}
