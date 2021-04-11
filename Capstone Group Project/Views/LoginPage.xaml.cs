using Capstone_Group_Project.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone_Group_Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        // Xamarin Forms uses the MVVM architecture:
        // Models/Program logic: the classes that contain data and do the actual work
        // Views: the XAML pages which handle how the GUI looks
        // ViewModels: an extension of the code-behind of each XAML page, which connects the Views to the Models/Program logic
    }
}