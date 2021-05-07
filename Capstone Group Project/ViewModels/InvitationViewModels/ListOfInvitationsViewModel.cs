using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.InvitationSystem;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class ListOfInvitationsViewModel
    {
        private static ListOfInvitationsViewModel currentInstance = null;
        public ObservableCollection<ConversationInvitation> ConversationInvitations { get; }
        public Command<ConversationInvitation> AcceptInvitationCommand { get; }
        public Command<ConversationInvitation> DeclineInvitationCommand { get; }
        public static int IdOfConversationInvitationLastTapped { get; set; }


        public ListOfInvitationsViewModel()
        {
            currentInstance = this;
            ConversationInvitations = new ObservableCollection<ConversationInvitation>();
            AcceptInvitationCommand = new Command<ConversationInvitation>(OnConversationInvitationAccepted);
            DeclineInvitationCommand = new Command<ConversationInvitation>(OnConversationInvitationDeclined);
            LoadAndDisplayAllConversationInvitations();
        }


        private void LoadAndDisplayAllConversationInvitations()
        {
            ConversationInvitation[] conversationInvitations = CurrentLoginState.GetConversationInvitations();
            ConversationInvitations.Clear();
            foreach (ConversationInvitation currentConversationInvitation in conversationInvitations)
            {
                ConversationInvitations.Add(currentConversationInvitation);
            }
        }


        private void OnConversationInvitationAccepted(ConversationInvitation conversationListing)
        {
            if (conversationListing == null)
                return;
            ReceivedInvitationHandler.AcceptConversationInvitation(conversationListing.Conversation_ID);
            ConversationInvitations.Remove(conversationListing);
        }


        private void OnConversationInvitationDeclined(ConversationInvitation conversationListing)
        {
            if (conversationListing == null)
                return;
            ReceivedInvitationHandler.DeclineConversationInvitation(conversationListing.Conversation_ID);
            ConversationInvitations.Remove(conversationListing);
        }


        public static void AddNewConversationInvitation(int conversationID)
        {
            ConversationInvitation newConversationInvitation = new ConversationInvitation()
            {
                Conversation_ID = conversationID
            };
            currentInstance.ConversationInvitations.Add(newConversationInvitation);
        }
    }
}
