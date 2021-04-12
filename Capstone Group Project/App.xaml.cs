using Capstone_Group_Project.Services;
using Capstone_Group_Project.Views;
using Xamarin.Forms;

namespace Capstone_Group_Project
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            //choose which startup page
            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
