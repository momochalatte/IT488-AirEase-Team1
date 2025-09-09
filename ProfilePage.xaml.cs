namespace IT488_Reg_Form;

public partial class ProfilePage : ContentPage
{
    private UserProfile? _profile; // Add this field

    // pass the created profile after sign-up
    public ProfilePage(UserProfile profile)
    {
        InitializeComponent();
        SetProfile(profile);



        ToolbarItems.Add(new ToolbarItem
        {
            Text = "Check In",
            Order = ToolbarItemOrder.Primary,
            Priority = 0,
            Command = new Command(async () => await Navigation.PushAsync(new CheckInPage()))
        });

      

    }

    // or load by email if you prefer
    public ProfilePage(string email)
    {
        InitializeComponent();
        _ = LoadAsync(email);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_profile is not null)
        {
            // Reload fresh data in case it was edited
            var fresh = await App.Database.GetByEmailAsync(_profile.Email);
            if (fresh != null) SetProfile(fresh);
        }
    }

    private async Task LoadAsync(string email)
    {
        var p = await App.Database.GetByEmailAsync(email.ToLowerInvariant());
        if (p != null) SetProfile(p);
    }

    private void SetProfile(UserProfile p)
    {
        _profile = p;
        NameLabel.Text = $"{p.FirstName} {p.LastName}".Trim();
        EmailLabel.Text = p.Email;
        if (!string.IsNullOrWhiteSpace(p.ProfileImagePath))
            AvatarImage!.Source = ImageSource.FromFile(p.ProfileImagePath);
    }

    async void Edit_Clicked(object sender, EventArgs e)
    {
        if (_profile is null) return;
        await Navigation.PushAsync(new EditProfilePage(_profile));
    }

    async void OpenRewards_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RewardsPage());
    }

    async void SignOut_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }


}
