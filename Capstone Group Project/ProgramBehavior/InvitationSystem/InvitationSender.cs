using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.InvitationSystem
{
    class InvitationSender
    {
        public static async Task<String> SendNewConversationInvitationToUserAccount(String accountUsername)
        {
            SendConversationInvitationRequestObject sendConversationInvitationRequestObject = new SendConversationInvitationRequestObject(accountUsername);
            HttpStatusCode resultStatusCode = HttpStatusCode.OK; //await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnHttpResponseCode(sendConversationInvitationRequestObject, "send_invitation.php");
            if (resultStatusCode == HttpStatusCode.OK)
                return "Successfully sent an invitation to " + accountUsername + " for conversation ID " + CurrentConversationState.GetCurrentConversationID() + ".";
            if (resultStatusCode == HttpStatusCode.NotFound)
                return "ERROR: the entered account username does not exist!";
            // If for some unknown reason we receive another status code, such as in the event of an error:
            return "ERROR: an invitation could not be delivered at this time!";
        }


        private class SendConversationInvitationRequestObject : CloudCommunicationObject
        {
            public String Recipient_Account_Username { get; set; } = null;
            public int Sender_Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public String Conversation_Private_Key { get; set; } = null;


            public SendConversationInvitationRequestObject(String recipientAccountUsername)
            {
                this.TaskRequested = "SEND_INVITATION";
                this.Recipient_Account_Username = recipientAccountUsername;
                this.Sender_Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.Conversation_ID = CurrentConversationState.GetCurrentConversationID();
                this.Conversation_Private_Key = CurrentConversationState.GetCurrentConversationPrivateKey();
            }
        }
    }
}
