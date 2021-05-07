using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;
using System;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem
{
    public class CurrentConversationState
    {
        private static int ConversationId = 0;
        private static Message[] RecentlyLoadedMessages = null;
        private static String ConversationPrivateKey = null;


        public static void SetCurrentConversationID(int conversationId)
        {
            ConversationId = conversationId;
        }


        public static int GetCurrentConversationID()
        {
            return ConversationId;
        }


        public static String GetCurrentConversationPrivateKey()
        {
            return ConversationPrivateKey;
        }


        public static async Task<Message[]> LoadInitialMessagesForCurrentConversation(int numberOfMessagesToLoad)
        {
            // While the cloud is not functional, we can just simply load some dummy messages for the current conversation:
            CreateAndReturnTestMessages(numberOfMessagesToLoad);
            return RecentlyLoadedMessages;

            LoadInitialMessagesAndPrivateKeyRequestObject loadInitialMessagesAndPrivateKeyRequestObject = new LoadInitialMessagesAndPrivateKeyRequestObject(numberOfMessagesToLoad);
            LoadMessagesAndPrivateKeyResponseObject cloudResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LoadMessagesAndPrivateKeyResponseObject>(loadInitialMessagesAndPrivateKeyRequestObject);
            RecentlyLoadedMessages = cloudResponseObject.Messages;
            ConversationPrivateKey = AsymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(cloudResponseObject.Conversation_Private_Key, CurrentLoginState.GetCurrentUserPrivateKey());
            foreach (Message currentMessage in RecentlyLoadedMessages)
            {
                String messageCiphertext = currentMessage.MessageBody;
                currentMessage.MessageBody = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(messageCiphertext, ConversationPrivateKey);
            }
            return RecentlyLoadedMessages;
        }


        public static async Task<Message[]> LoadRangeOfMessagesForCurrentConversation(int startingMessageNumberInclusive, int endingMessageNumberInclusive)
        {
            // While the cloud is not functional, we can just simply load some dummy messages for the current conversation:
            CreateAndReturnTestMessages(25);
            return RecentlyLoadedMessages;

            LoadSpecifiedMessagesAndPrivateKeyRequestObject loadSpecifiedMessagesAndPrivateKeyRequestObject = new LoadSpecifiedMessagesAndPrivateKeyRequestObject(startingMessageNumberInclusive, endingMessageNumberInclusive);
            LoadMessagesAndPrivateKeyResponseObject cloudResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LoadMessagesAndPrivateKeyResponseObject>(loadSpecifiedMessagesAndPrivateKeyRequestObject);
            String conversationPrivateKey = AsymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(cloudResponseObject.Conversation_Private_Key, CurrentLoginState.GetCurrentUserPrivateKey());
            foreach (Message currentEncryptedMessage in cloudResponseObject.Messages)
            {
                String messageCiphertext = currentEncryptedMessage.MessageBody;
                currentEncryptedMessage.MessageBody = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(messageCiphertext, conversationPrivateKey);
            }
            RecentlyLoadedMessages = cloudResponseObject.Messages;
            ConversationPrivateKey = conversationPrivateKey;
            return RecentlyLoadedMessages;
        }


        private static void CreateAndReturnTestMessages(int numberOfMessagesToLoad)
        {
            RecentlyLoadedMessages = new Message[numberOfMessagesToLoad];
            for (int messageNumber = 1; messageNumber <= numberOfMessagesToLoad; ++messageNumber)
            {
                Message currentMessage = new Message()
                {
                    MessageSenderUsername = "Jamal",
                    TimeAndDateMessageWasSent = "4/9/2021",
                    MessageBody = "TEST MESSAGE #" + messageNumber + " FOR CONVERSATION #" + ConversationId + "blah blah blah blah blah blah blah blah"
                };
                RecentlyLoadedMessages[messageNumber - 1] = currentMessage;
            }
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
                this.Conversation_ID = CurrentConversationState.ConversationId;
                this.NumberOfMessagesToLoad = numberOfMessagesToLoad;
            }
        }


        private class LoadMessagesAndPrivateKeyResponseObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public Message[] Messages { get; set; } = null;
            public String Conversation_Private_Key { get; set; } = null;
        }


        private class LoadSpecifiedMessagesAndPrivateKeyRequestObject : CloudCommunicationObject
        {
            public int Account_ID { get; set; } = 0;
            public int Conversation_ID { get; set; } = 0;
            public int StartingMessageNumberInclusive { get; set; } = 0;
            public int EndingMessageNumberInclusive { get; set; } = 0;


            public LoadSpecifiedMessagesAndPrivateKeyRequestObject(int startingMessageNumberInclusive, int endingMessageNumberInclusive)
            {
                this.TaskRequested = "LOAD_SPECIFIED_MESSAGE_RANGE_AND_PRIVATE_KEY";
                this.Account_ID = CurrentLoginState.GetCurrentUserAccountID(); ;
                this.Conversation_ID = CurrentConversationState.ConversationId;
                this.StartingMessageNumberInclusive = startingMessageNumberInclusive;
                this.EndingMessageNumberInclusive = endingMessageNumberInclusive;
            }
        }
    }
}
