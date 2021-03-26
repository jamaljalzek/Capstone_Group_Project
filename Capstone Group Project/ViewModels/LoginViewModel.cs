using Capstone_Group_Project.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public String EnteredUsername { get; set; }
        public String EnteredPassword { get; set; }
        public Command LoginCommand { get; }
        public Command NavigateToRegisterPageCommand { get; }


        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            NavigateToRegisterPageCommand = new Command(NavigateToRegisterPage);
        }


        private async void OnLoginClicked(object obj)
        {
            // Prefixing with "//" switches to a different navigation stack instead of pushing to the active one:
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }


        private async void NavigateToRegisterPage()
        {
            // We dont prefix with "//", which causes an exception in this case:
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
