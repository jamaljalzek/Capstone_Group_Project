using Capstone_Group_Project.ProgramBehavior.InvitationSystem;
using System;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels.InvitationViewModels
{
    class InviteNewParticipantViewModel : BaseViewModel
    {
        public String EnteredUsername { get; set; } = "";
        public Command InviteCommand { get; set; } = null;
        public Command BackCommand { get; set; } = null;
        public String DisplayedStatusMessage { get; set; } = "";


        public InviteNewParticipantViewModel()
        {
            InviteCommand = new Command(SendNewInvitationToEnteredUserAccount);
            BackCommand = new Command(GoBack);
        }


        private async void SendNewInvitationToEnteredUserAccount()
        {
            if (EnteredUsername == null || EnteredUsername.Length == 0)
                return;
            DisplayedStatusMessage = await InvitationSender.SendNewConversationInvitationToUserAccount(EnteredUsername);
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedStatusMessage");
        }


        private async void GoBack()
        {
            // This will pop the current page off the navigation stack:
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
