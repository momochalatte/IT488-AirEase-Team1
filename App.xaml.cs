
using System.IO;


namespace IT488_Reg_Form;


   

    public partial class App : Application
    {

        // Create/open DB at: <AppDataDirectory>\profiles.db3
        public static ProfileDatabase Database { get; } =
            new ProfileDatabase(Path.Combine(FileSystem.AppDataDirectory, "profiles.db3"));



        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SignInPage())
            {
                BarBackgroundColor = Color.FromRgb(36, 160, 237), 
                BarTextColor = Colors.White
            };

    }
    }
