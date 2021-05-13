using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.InvitationSystem
{
    class InvitationSender
    {
        public static async Task<String> SendNewConversationInvitationToUserAccount(String accountUsername)
        {
            String attemptToGetPublicKey = await GetPublicKeyOfGivenAccountUsername(accountUsername);
            if (attemptToGetPublicKey == null)
                return "ERROR: the entered account username does not exist!";
            SendConversationInvitationRequestObject sendConversationInvitationRequestObject = new SendConversationInvitationRequestObject(accountUsername, attemptToGetPublicKey);
            HttpStatusCode resultStatusCode = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnHttpResponseCode(sendConversationInvitationRequestObject, "send_invitation.php");
            if (resultStatusCode == HttpStatusCode.OK)
                return "Successfully sent an invitation to " + accountUsername + " for conversation ID " + CurrentConversationState.GetCurrentConversationID() + ".";
            if (resultStatusCode == HttpStatusCode.Conflict)
                return "ERROR: the entered account username is already a participant in the conversation!";
            // If for some unknown reason we receive another status code, such as in the event of an error:
            return "ERROR: an invitation could not be delivered at this time!";
        }


        private static async Task<String> GetPublicKeyOfGivenAccountUsername(String accountUsername)
        {
            LookUpGivenUsernamePublicKeyObject lookUpGivenUsernamePublicKeyObject = new LookUpGivenUsernamePublicKeyObject(accountUsername);
            LookUpGivenUsernamePublicKeyObject resultOfLookUpRequest = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LookUpGivenUsernamePublicKeyObject>(lookUpGivenUsernamePublicKeyObject);
            if (resultOfLookUpRequest.ResultOfRequest.Equals("USERNAME_NOT_FOUND"))
                return null;
            return resultOfLookUpRequest.Public_Key;
        }


        private class LookUpGivenUsernamePublicKeyObject : CloudCommunicationObject
        {
            public String Account_Username { get; set; } = null;
            public String Public_Key { get; set; } = null;


            public LookUpGivenUsernamePublicKeyObject(String recipientAccountUsername)
            {
                this.TaskRequested = "LOOKUP_PUBLIC_KEY";
                this.Account_Username = recipientAccountUsername;
            }
        }


        private class SendConversationInvitationRequestObject : CloudCommunicationObject
        {
            public String Recipient_Account_Username { get; set; } = null;
            public int Sender_Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public String Conversation_Private_Key { get; set; } = null;


            public SendConversationInvitationRequestObject(String recipientAccountUsername, String recipientPublicKey)
            {
                this.TaskRequested = "SEND_INVITATION";
                this.Recipient_Account_Username = recipientAccountUsername;
                this.Sender_Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.Conversation_ID = CurrentConversationState.GetCurrentConversationID();
                this.Conversation_Private_Key = AsymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String(CurrentConversationState.GetCurrentConversationPrivateKey(), recipientPublicKey);
            }
        }
    }
}
