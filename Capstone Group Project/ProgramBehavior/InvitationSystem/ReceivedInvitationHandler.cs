using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;

namespace Capstone_Group_Project.ProgramBehavior.InvitationSystem
{
    class ReceivedInvitationHandler
    {
        public static void AcceptConversationInvitation(int conversationIdOfInvitation)
        {
            AcceptConversationInvitationRequestObject acceptConversationInvitationRequestObject = new AcceptConversationInvitationRequestObject(conversationIdOfInvitation);
        }


        public static void DeclineConversationInvitation(int conversationIdOfInvitation)
        {
            DeclineConversationInvitationRequestObject declineConversationInvitationRequestObject = new DeclineConversationInvitationRequestObject(conversationIdOfInvitation);
            MobileApplicationHttpClient.PostObjectAsynchronouslyWithoutWaitingForResponse(declineConversationInvitationRequestObject);
        }


        private class AcceptConversationInvitationRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;


            public AcceptConversationInvitationRequestObject(int conversationIdOfInvitation)
            {
                this.TaskRequested = "ACCEPT_INVITATION";
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.Conversation_ID = conversationIdOfInvitation;
            }
        }


        private class DeclineConversationInvitationRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;


            public DeclineConversationInvitationRequestObject(int conversationIdOfInvitation)
            {
                this.TaskRequested = "DECLINE_INVITATION";
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.Conversation_ID = conversationIdOfInvitation;
            }
        }
    }
}
