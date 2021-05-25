using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.InvitationSystem;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class ListOfInvitationsViewModel
    {
        public ObservableCollection<ConversationInvitation> ConversationInvitations { get; }
        public Command CheckForNewInvitationsCommand { get; }
        public Command<ConversationInvitation> AcceptInvitationCommand { get; }
        public Command<ConversationInvitation> DeclineInvitationCommand { get; }
        public static int IdOfConversationInvitationLastTapped { get; set; }


        public ListOfInvitationsViewModel()
        {
            ConversationInvitations = new ObservableCollection<ConversationInvitation>();
            CheckForNewInvitationsCommand = new Command(OnCheckForNewInvitations);
            AcceptInvitationCommand = new Command<ConversationInvitation>(OnConversationInvitationAccepted);
            DeclineInvitationCommand = new Command<ConversationInvitation>(OnConversationInvitationDeclined);
            LoadAndDisplayAllConversationInvitations();
        }


        private async void OnCheckForNewInvitations()
        {
            ConversationInvitation[] conversationInvitations = await ConversationInvitationHandler.CheckForNewConversationInvitations();
            foreach (ConversationInvitation currentConversationInvitation in conversationInvitations)
            {
                ConversationInvitations.Add(currentConversationInvitation);
            }
        }


        private void LoadAndDisplayAllConversationInvitations()
        {
            List<ConversationInvitation> conversationInvitations = CurrentLoginState.GetConversationInvitations();
            ConversationInvitations.Clear();
            foreach (ConversationInvitation currentConversationInvitation in conversationInvitations)
            {
                ConversationInvitations.Add(currentConversationInvitation);
            }
        }


        private async void OnConversationInvitationAccepted(ConversationInvitation invitationListing)
        {
            if (invitationListing == null)
                return;
            bool wasAttemptSuccessful = await ConversationInvitationHandler.AcceptConversationInvitation(invitationListing.Conversation_ID);
            if (wasAttemptSuccessful)
                ConversationInvitations.Remove(invitationListing);
        }


        private async void OnConversationInvitationDeclined(ConversationInvitation invitationListing)
        {
            if (invitationListing == null)
                return;
            bool wasAttemptSuccessful = await ConversationInvitationHandler.DeclineConversationInvitation(invitationListing.Conversation_ID);
            if (wasAttemptSuccessful)
                ConversationInvitations.Remove(invitationListing);
        }
    }
}
