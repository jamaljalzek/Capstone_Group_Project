using Capstone_Group_Project.ProgramBehavior.UserAccountSystem.UserAccountLoginSystem;
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
        public String DisplayedStatusMessage { get; set; }
        public Command LoginCommand { get; }
        public Command NavigateToRegisterPageCommand { get; }


        public LoginViewModel()
        {
            LoginCommand = new Command(AttemptToLogUserInWithEnteredInformation);
            NavigateToRegisterPageCommand = new Command(NavigateToRegisterPage);
        }


        private async void AttemptToLogUserInWithEnteredInformation(object obj)
        {
            if (EnteredUsername == null || EnteredPassword == null)
                return;
            DisplayedStatusMessage = "Attempting to log in...";
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedStatusMessage");
            bool wasUserSuccessfullyLoggedIn = await UserAccountLoginHandler.AttemptToLogUserIn(EnteredUsername, EnteredPassword);
            if (wasUserSuccessfullyLoggedIn)
                NavigateToAboutPage();
            else
            {
                DisplayedStatusMessage = UserAccountLoginHandler.errorMessage;
                UpdateUserInterfaceElementBoundToGivenVariable("DisplayedStatusMessage");
            }
        }


        private void UpdateUserInterfaceElementBoundToGivenVariable(String nameOfVariable)
        {
            // I'm wrapping this method with a more descriptive name for clarity.
            OnPropertyChanged(nameOfVariable);
        }


        private async void NavigateToAboutPage()
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
