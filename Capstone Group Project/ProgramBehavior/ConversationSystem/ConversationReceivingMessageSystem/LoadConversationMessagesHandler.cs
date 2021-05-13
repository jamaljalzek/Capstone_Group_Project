using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;
using System;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.ConversationSystem.ConversationReceivingMessageSystem
{
    class LoadConversationMessagesHandler
    {
        private static Message[] RecentlyLoadedMessages = null;


        private static void CreateAndReturnTestMessages(int numberOfMessagesToLoad)
        {
            RecentlyLoadedMessages = new Message[numberOfMessagesToLoad];
            DateTime currentDateTime = DateTime.Now;
            for (int messageNumber = 1; messageNumber <= numberOfMessagesToLoad; ++messageNumber)
            {
                Message currentMessage = new Message()
                {
                    MessageSenderUsername = "Jamal",
                    TimeAndDateMessageWasSent = currentDateTime.ToString(),
                    MessageBody = "TEST MESSAGE #" + messageNumber
                };
                RecentlyLoadedMessages[messageNumber - 1] = currentMessage;
            }
        }


        public static async Task<Message[]> LoadInitialMessagesForCurrentConversation(int numberOfMessagesToLoad)
        {
            // While the cloud is not functional, we can just simply load some dummy messages for the current conversation:
            CreateAndReturnTestMessages(numberOfMessagesToLoad);
            return RecentlyLoadedMessages;

            LoadInitialMessagesAndPrivateKeyRequestObject loadInitialMessagesAndPrivateKeyRequestObject = new LoadInitialMessagesAndPrivateKeyRequestObject(numberOfMessagesToLoad);
            LoadInitialMessagesAndPrivateKeyResponseObject cloudResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LoadInitialMessagesAndPrivateKeyResponseObject>(loadInitialMessagesAndPrivateKeyRequestObject);
            String conversationPrivateKey = AsymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(cloudResponseObject.Conversation_Private_Key, CurrentLoginState.GetCurrentUserPrivateKey());
            CurrentConversationState.SetCurrentConversationPrivateKey(conversationPrivateKey);
            foreach (Message currentMessage in cloudResponseObject.Messages)
            {
                DateTime currentMessageUtcDateTime = DateTime.Parse(currentMessage.TimeAndDateMessageWasSent);
                currentMessage.TimeAndDateMessageWasSent = currentMessageUtcDateTime.ToLocalTime().ToString();
                String messageCiphertext = currentMessage.MessageBody;
                currentMessage.MessageBody = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(messageCiphertext, conversationPrivateKey);
            }
            CurrentConversationState.SetCurrentConversationInitialSetOfMessages(cloudResponseObject.Messages);
            return cloudResponseObject.Messages;
        }


        public static async Task<Message[]> LoadRangeOfMessagesForCurrentConversation(int startingMessageNumberInclusive, int endingMessageNumberInclusive)
        {
            // While the cloud is not functional, we can just simply load some dummy messages for the current conversation:
            CreateAndReturnTestMessages(25);
            return RecentlyLoadedMessages;

            LoadSpecifiedMessagesRequestObject loadSpecifiedMessagesAndPrivateKeyRequestObject = new LoadSpecifiedMessagesRequestObject(startingMessageNumberInclusive, endingMessageNumberInclusive);
            LoadMessagesResponseObject cloudResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LoadMessagesResponseObject>(loadSpecifiedMessagesAndPrivateKeyRequestObject);
            foreach (Message currentEncryptedMessage in cloudResponseObject.Messages)
            {
                String messageCiphertext = currentEncryptedMessage.MessageBody;
                currentEncryptedMessage.MessageBody = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(messageCiphertext, CurrentConversationState.GetCurrentConversationPrivateKey());
            }
            CurrentConversationState.AddToCurrentConversationLoadedMessages(cloudResponseObject.Messages);
            return cloudResponseObject.Messages;
        }


        public static async Task<Message[]> LoadAnyNewMessagesForCurrentConversation(int loadedMessagesEndingMessageNumberInclusive)
        {
            LoadNewMessagesRequestObject loadInitialMessagesAndPrivateKeyRequestObject = new LoadNewMessagesRequestObject(loadedMessagesEndingMessageNumberInclusive + 1);
            LoadMessagesResponseObject cloudResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LoadMessagesResponseObject>(loadInitialMessagesAndPrivateKeyRequestObject);
            foreach (Message currentMessage in cloudResponseObject.Messages)
            {
                String messageCiphertext = currentMessage.MessageBody;
                currentMessage.MessageBody = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(messageCiphertext, CurrentConversationState.GetCurrentConversationPrivateKey());
            }
            CurrentConversationState.AddToCurrentConversationLoadedMessages(cloudResponseObject.Messages);
            return cloudResponseObject.Messages;
        }


        private class LoadInitialMessagesAndPrivateKeyRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public int NumberOfMessagesToLoad { get; set; } = 0;


            public LoadInitialMessagesAndPrivateKeyRequestObject(int numberOfMessagesToLoad)
            {
                this.TaskRequested = "LOAD_MOST_RECENT_MESSAGES_AND_PRIVATE_KEY";
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID();
                this.Conversation_ID = CurrentConversationState.GetCurrentConversationID();
                this.NumberOfMessagesToLoad = numberOfMessagesToLoad;
            }
        }


        private class LoadInitialMessagesAndPrivateKeyResponseObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public Message[] Messages { get; set; } = null;
            public String Conversation_Private_Key { get; set; } = null;
        }


        private class LoadSpecifiedMessagesRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public int StartingMessageNumberInclusive { get; set; } = 0;
            public int EndingMessageNumberInclusive { get; set; } = 0;


            public LoadSpecifiedMessagesRequestObject(int startingMessageNumberInclusive, int endingMessageNumberInclusive)
            {
                this.TaskRequested = "LOAD_SPECIFIED_MESSAGE_RANGE";
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID(); ;
                this.Conversation_ID = CurrentConversationState.GetCurrentConversationID();
                this.StartingMessageNumberInclusive = startingMessageNumberInclusive;
                this.EndingMessageNumberInclusive = endingMessageNumberInclusive;
            }
        }


        private class LoadMessagesResponseObject : CloudCommunicationObject
        {
            public Message[] Messages { get; set; } = null;
        }


        private class LoadNewMessagesRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public int StartingMessageNumberInclusive { get; set; } = 0;


            public LoadNewMessagesRequestObject(int startingMessageNumberInclusive)
            {
                this.TaskRequested = "LOAD_ANY_NEW_MESSAGES";
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID(); ;
                this.Conversation_ID = CurrentConversationState.GetCurrentConversationID();
                this.StartingMessageNumberInclusive = startingMessageNumberInclusive;
            }
        }
    }
}
