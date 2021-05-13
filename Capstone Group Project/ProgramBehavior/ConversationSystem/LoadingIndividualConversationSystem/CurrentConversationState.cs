using Capstone_Group_Project.Models;
using System;
using System.Collections.Generic;

namespace Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem
{
    public class CurrentConversationState
    {
        private static int ConversationId = 0;
        private static String ConversationPrivateKey = null;
        private static List<Message> AllLoadedMessages = null;


        public static void SetCurrentConversationID(int conversationId)
        {
            ConversationId = conversationId;
        }


        public static int GetCurrentConversationID()
        {
            return ConversationId;
        }


        public static void SetCurrentConversationPrivateKey(String conversationPrivateKey)
        {
            ConversationPrivateKey = conversationPrivateKey;
        }


        public static String GetCurrentConversationPrivateKey()
        {
            return ConversationPrivateKey;
        }


        public static void SetCurrentConversationInitialSetOfMessages(Message[] initialSetOfMessages)
        {
            AllLoadedMessages = new List<Message>(initialSetOfMessages);
        }


        public static void AddToCurrentConversationLoadedMessages(Message[] setOfMessagesToAdd)
        {
            AllLoadedMessages.AddRange(setOfMessagesToAdd);
        }


        public static void AddToCurrentConversationLoadedMessages(Message newMessageToAdd)
        {
            AllLoadedMessages.Add(newMessageToAdd);
        }


        public static List<Message> GetCurrentConversationLoadedMessages()
        {
            return AllLoadedMessages;
        }
    }
}
