using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;
using System;

namespace Capstone_Group_Project.ProgramBehavior.ConversationSystem.ConversationSendingMessageSystem
{
    class SendConversationMessageHandler
    {
        public static void SendIndividualMessageToAllConversationParticipants(String messageBody, DateTime dateAndTimeMessageWasSent)
        {
            SendMessageRequestObject sendMessageRequestObject = new SendMessageRequestObject(messageBody, dateAndTimeMessageWasSent);
            MobileApplicationHttpClient.PostObjectAsynchronouslyWithoutWaitingForResponse(sendMessageRequestObject);
        }


        private class SendMessageRequestObject : CloudCommunicationObject
        {
            public int Sender_Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public String Sent_Date_And_Time { get; set; } = null;
            public String Message_Ciphertext { get; set; } = null;


            public SendMessageRequestObject(String messagePlaintext, DateTime dateAndTimeMessageWasSent)
            {
                Sender_Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                Conversation_ID = CurrentConversationState.GetCurrentConversationID();
                Sent_Date_And_Time = dateAndTimeMessageWasSent.ToUniversalTime().ToString();
                Message_Ciphertext = SymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String(messagePlaintext, CurrentConversationState.GetCurrentConversationPrivateKey());
            }
        }
    }
}
