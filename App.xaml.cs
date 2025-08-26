

namespace IT488_Reg_Form

{
   

    public partial class App : Application
    {


     


        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new IT488_Reg_Form());
        }
    }
}