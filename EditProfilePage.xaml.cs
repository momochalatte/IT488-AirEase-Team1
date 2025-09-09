using System.Text.RegularExpressions;

namespace IT488_Reg_Form;

public partial class EditProfilePage : ContentPage
{
    private UserProfile _profile;           // the current profile being edited
    private string? _pickedImagePath;       // temp path for newly picked photo

    public EditProfilePage(UserProfile profile)
    {
        InitializeComponent();
        _profile = profile;
        PrefillFields(profile);
    }

    void PrefillFields(UserProfile p)
    {
        FirstNameEntry.Text = p.FirstName;
        LastNameEntry.Text = p.LastName;
        DobPicker.Date = p.DateOfBirth;
        AddressEntry.Text = p.Address;
        EmailEntry.Text = p.Email;
        PhoneEntry.Text = p.Phone;

        if (!string.IsNullOrWhiteSpace(p.ProfileImagePath))
            ProfileImage.Source = ImageSource.FromFile(p.ProfileImagePath);
    }

    async void PickPhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Choose a profile picture",
                FileTypes = FilePickerFileType.Images
            });

            if (result is null) return;

            // copy to app data so we keep a stable path
            var dest = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
            using (var src = await result.OpenReadAsync())
            using (var dst = File.Open(dest, FileMode.Create, FileAccess.Write))
                await src.CopyToAsync(dst);

            _pickedImagePath = dest;
            ProfileImage.Source = ImageSource.FromFile(dest);
            PhotoError.IsVisible = false;
        }
        catch (Exception ex)
        {
            PhotoError.Text = ex.Message;
            PhotoError.IsVisible = true;
        }
    }

    async void Save_Clicked(object sender, EventArgs e)
    {
        Busy.IsVisible = Busy.IsRunning = true;
        FormError.IsVisible = PasswordError.IsVisible = DobError.IsVisible = false;

        try
        {
            // 1) Gather + trim
            var first = (FirstNameEntry.Text ?? "").Trim();
            var last = (LastNameEntry.Text ?? "").Trim();
            var dob = DobPicker.Date;
            var addr = (AddressEntry.Text ?? "").Trim();
            var email = (EmailEntry.Text ?? "").Trim().ToLowerInvariant();
            var phone = (PhoneEntry.Text ?? "").Trim();

            // 2) Validate basics
            if (first.Length < 2 || last.Length < 2) { ShowFormError("First and last name must be at least 2 characters."); return; }
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { ShowFormError("Enter a valid email address."); return; }
            if (!Regex.IsMatch(phone, @"^\+?[0-9]{10,15}$")) { ShowFormError("Phone must be 10–15 digits (optional leading +)."); return; }

            // 18+ rule
            var minDob = DateTime.Today.AddYears(-18);
            if (dob > minDob)
            {
                DobError.Text = "You must be at least 18 years old.";
                DobError.IsVisible = true;
                return;
            }

            // 3) If email changed, ensure not taken
            if (!string.Equals(email, _profile.Email, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await App.Database.GetByEmailAsync(email);
                if (exists is not null) { ShowFormError("That email is already in use by another account."); return; }
            }

            // 4) Optional password change
            var currentPwd = CurrentPasswordEntry.Text ?? "";
            var newPwd = NewPasswordEntry.Text ?? "";
            var confirmPwd = ConfirmPasswordEntry.Text ?? "";

            if (!string.IsNullOrWhiteSpace(newPwd) || !string.IsNullOrWhiteSpace(confirmPwd) || !string.IsNullOrWhiteSpace(currentPwd))
            {
                // must verify current password
                if (string.IsNullOrWhiteSpace(currentPwd))
                {
                    PasswordError.Text = "Enter your current password to set a new one.";
                    PasswordError.IsVisible = true;
                    return;
                }

                // verify current
                var isHash = !string.IsNullOrEmpty(_profile.PasswordHash) && _profile.PasswordHash.StartsWith("$2");
                var ok = isHash
                    ? BCrypt.Net.BCrypt.Verify(currentPwd, _profile.PasswordHash)
                    : string.Equals(_profile.PasswordHash ?? "", currentPwd, StringComparison.Ordinal);

                if (!ok)
                {
                    PasswordError.Text = "Current password is incorrect.";
                    PasswordError.IsVisible = true;
                    return;
                }

                // validate new
                bool strong = newPwd.Length >= 8 && newPwd.Any(char.IsUpper) && newPwd.Any(char.IsDigit) && newPwd.Any(c => !char.IsLetterOrDigit(c));
                if (!strong)
                {
                    PasswordError.Text = "New password must be 8+ chars, include uppercase, digit, and symbol.";
                    PasswordError.IsVisible = true;
                    return;
                }
                if (!string.Equals(newPwd, confirmPwd, StringComparison.Ordinal))
                {
                    PasswordError.Text = "Passwords do not match.";
                    PasswordError.IsVisible = true;
                    return;
                }

                _profile.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPwd);
            }

            // 5) Apply updates
            _profile.FirstName = first;
            _profile.LastName = last;
            _profile.DateOfBirth = dob;
            _profile.Address = addr;
            _profile.Email = email;
            _profile.Phone = phone;

            if (!string.IsNullOrWhiteSpace(_pickedImagePath))
                _profile.ProfileImagePath = _pickedImagePath;

            // 6) Persist
            await App.Database.UpdateAsync(_profile);

            await DisplayAlert("Saved", "Your profile has been updated.", "OK");
            await Navigation.PopAsync(); // return to ProfilePage; it refreshes in OnAppearing
        }
        catch (Exception ex)
        {
            ShowFormError(ex.Message);
        }
        finally
        {
            Busy.IsVisible = Busy.IsRunning = false;
        }
    }

    void Cancel_Clicked(object sender, EventArgs e) => Navigation.PopAsync();

    // Helper now returns void (not bool)
    void ShowFormError(string msg)
    {
        FormError.Text = msg;
        FormError.IsVisible = true;
    }
}
