using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;
using Capstone_Group_Project.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.InvitationSystem
{
    class ConversationInvitationHandler
    {
        public static async Task<ConversationInvitation[]> CheckForNewConversationInvitations()
        {
            int startingConversationIdOfAnyNewInvitationsInclusive = CurrentLoginState.GetConversationIdOfMostRecentlyLoadedInvitation() + 1;
            LoadNewConversationInvitationsRequestObject loadNewConversationInvitationsRequestObject = new LoadNewConversationInvitationsRequestObject(startingConversationIdOfAnyNewInvitationsInclusive);
            LoadNewConversationInvitationsReponseObject loadNewConversationInvitationsReponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LoadNewConversationInvitationsReponseObject>(loadNewConversationInvitationsRequestObject, "refresh_invitations.php");
            CurrentLoginState.AddNewConversationInvitations(loadNewConversationInvitationsReponseObject.ConversationInvitations);
            return loadNewConversationInvitationsReponseObject.ConversationInvitations;
        }


        public static async Task<bool> AcceptConversationInvitation(int conversationIdOfInvitation)
        {
            AcceptOrDeclineConversationInvitationRequestObject acceptConversationInvitationRequestObject = new AcceptOrDeclineConversationInvitationRequestObject("ACCEPT_INVITATION", conversationIdOfInvitation);
            HttpStatusCode resultOfAttempt = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnHttpResponseCode(acceptConversationInvitationRequestObject, "accept_invitation.php");
            if (resultOfAttempt == HttpStatusCode.OK)
            {
                CurrentLoginState.AddIdToListOfConversationsCurrentUserIsParticipantIn(conversationIdOfInvitation);
                ListOfConversationsViewModel.AddNewConversationListingToDisplay(conversationIdOfInvitation);
                CurrentLoginState.RemoveConversationInvitation(conversationIdOfInvitation);
                return true;
            }
            return false;
        }


        public static async Task<bool> DeclineConversationInvitation(int conversationIdOfInvitation)
        {
            AcceptOrDeclineConversationInvitationRequestObject declineConversationInvitationRequestObject = new AcceptOrDeclineConversationInvitationRequestObject("DECLINE_INVITATION", conversationIdOfInvitation);
            HttpStatusCode resultOfAttempt = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnHttpResponseCode(declineConversationInvitationRequestObject, "decline_invitation.php");
            if (resultOfAttempt == HttpStatusCode.OK)
            {
                CurrentLoginState.RemoveConversationInvitation(conversationIdOfInvitation);
                return true;
            }
            return false;
        }


        private class LoadNewConversationInvitationsRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int StartingConversationIdInclusive { get; set; } = 0;


            public LoadNewConversationInvitationsRequestObject(int startingConversationIdInclusive)
            {
                this.TaskRequested = "LOAD_ANY_NEW_INVITATIONS";
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.StartingConversationIdInclusive = startingConversationIdInclusive;
            }
        }


        private class LoadNewConversationInvitationsReponseObject : CloudCommunicationObject
        {
            public ConversationInvitation[] ConversationInvitations { get; set; } = null;
        }


        private class AcceptOrDeclineConversationInvitationRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;


            public AcceptOrDeclineConversationInvitationRequestObject(String acceptOrDeclineInvitation, int conversationIdOfInvitation)
            {
                this.TaskRequested = acceptOrDeclineInvitation;
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.Conversation_ID = conversationIdOfInvitation;
            }
        }
    }
}
