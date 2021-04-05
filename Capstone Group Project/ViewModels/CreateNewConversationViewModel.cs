using Capstone_Group_Project.ProgramBehavior.ConversationSystem.ConversationCreationSystem;
using System;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class CreateNewConversationViewModel : BaseViewModel
    {
        public String DisplayedStatusMessage { get; set; }
        public Command CancelCommand { get; }


        public CreateNewConversationViewModel()
        {
            // This constructor is called every single time we launch the CreateNewConversationPage.
            // Thus, everything inside of it will immediately run automatically every time the user navigates to the CreateNewConversationPage.
            CancelCommand = new Command(CancelConversationCreation);
            DisplayedStatusMessage = "Creating new conversation...";
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedStatusMessage");
            AttemptToCreateNewConversationAndDisplayResult();
        }


        private async void CancelConversationCreation()
        {
            // This will pop the current page off the navigation stack:
            await Shell.Current.GoToAsync("..");
        }


        private async void AttemptToCreateNewConversationAndDisplayResult()
        {
            // We immediately display the result of attempting to create the new conversation.
            // This will either be a success message containing the new conversation's Conversation_ID number,
            // or an error message (which specifies what the error is and why):
            DisplayedStatusMessage = await CreateNewConversationHandler.AttemptToCreateNewConversationWithUserAsSoleParticipant();
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedStatusMessage");
        }
    }
}
