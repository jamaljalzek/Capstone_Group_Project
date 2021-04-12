using Capstone_Group_Project.Program_Behavior.User_Account_System.User_Account_Creation_System;
using System;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        public Command CreateNewAccountCommand { get; }
        public String EnteredUsername { get; set; }
        public String FirstEnteredPassword { get; set; }
        public String SecondEnteredPassword { get; set; }
        public String DisplayedStatusMessage { get; set; }


        public RegisterViewModel()
        {
            CreateNewAccountCommand = new Command(AttemptToCreateNewUserAccountWithEnteredInformation);
        }


        private async void AttemptToCreateNewUserAccountWithEnteredInformation(object obj)
        {
            if (EnteredUsername == null || FirstEnteredPassword == null || SecondEnteredPassword == null)
                return;
            DisplayedStatusMessage = "Checking entered account details...";
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedStatusMessage");
            // We immediately display the result of attempting to create the new user account,
            // which will either be a success message, or an error message (which specifies what the error is and why):
            DisplayedStatusMessage = await UserAccountCreator.AttemptToCreateNewUserAccount(EnteredUsername, FirstEnteredPassword, SecondEnteredPassword);
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedStatusMessage");
        }
    }
}
