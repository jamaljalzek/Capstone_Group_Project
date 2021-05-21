using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.ConversationSystem.ConversationSendingMessageSystem
{
    class SendConversationMessageHandler
    {
        public static async Task<bool> SendIndividualMessageToAllConversationParticipants(String messageBody, DateTime dateAndTimeMessageWasSent)
        {
            SendMessageRequestObject sendMessageRequestObject = new SendMessageRequestObject(messageBody, dateAndTimeMessageWasSent);
            HttpStatusCode resultCode = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnHttpResponseCode(sendMessageRequestObject, "send_message.php");
            if (resultCode == HttpStatusCode.OK)
            {
                /*                Message newMessage = new Message()
                                {
                                    MessageSenderUsername = null,
                                    TimeAndDateMessageWasSent = dateAndTimeMessageWasSent.ToString(),
                                    MessageBody = messageBody
                                };
                                CurrentConversationState.AddToCurrentConversationLoadedMessages(newMessage);*/
                return true;
            }
            return false;
        }


        private class SendMessageRequestObject : CloudCommunicationObject
        {
            public int Sender_Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public String Sent_Date_And_Time { get; set; } = null;
            public String Message_Ciphertext { get; set; } = null;


            public SendMessageRequestObject(String messagePlaintext, DateTime dateAndTimeMessageWasSent)
            {
                this.TaskRequested = "SEND_NEW_MESSAGE";
                this.Sender_Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.Conversation_ID = CurrentConversationState.GetCurrentConversationID();
                this.Sent_Date_And_Time = dateAndTimeMessageWasSent.ToUniversalTime().ToString();
                this.Message_Ciphertext = SymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String(messagePlaintext, CurrentConversationState.GetCurrentConversationPrivateKey());
            }
        }
    }
}
