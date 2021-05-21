using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using System;
using System.Collections.Generic;

namespace Capstone_Group_Project.ProgramBehavior.UserAccountSystem
{
    public class CurrentLoginState
    {
        private static CurrentLoginState currentLoginState;

        public int Account_ID { get; set; } = 0;
        public String Public_Key { get; set; } = null;
        public String Private_Key { get; set; } = null;
        public List<int> IdsOfConversationsUserIsParticipantIn { get; set; } = null;
        public List<ConversationInvitation> ConversationInvitations { get; set; } = null;


        // For the time being, while the cloud is not functional we will just set up a dummy login state for testing purposes:
        static CurrentLoginState()
        {
            currentLoginState = new CurrentLoginState();
            currentLoginState.IdsOfConversationsUserIsParticipantIn = new List<int>() { 111, 222, 333, 444, 555, 666, 777 };
            currentLoginState.ConversationInvitations = new List<ConversationInvitation>(5);
            for (int currentInvitationNumber = 0; currentInvitationNumber < 5; ++currentInvitationNumber)
            {
                ConversationInvitation newInvitation = new ConversationInvitation()
                {
                    Conversation_ID = currentInvitationNumber * 1000,
                    Account_Username = "AccountUsername" + currentInvitationNumber
                };
                currentLoginState.ConversationInvitations.Add(newInvitation);
            }
        }


        public static void LoadNewLoginState(LogUserIntoAccountResponseObject loginAttemptResponseFromCloudObject, String enteredPassword)
        {
            currentLoginState = new CurrentLoginState();
            currentLoginState.Account_ID = loginAttemptResponseFromCloudObject.Account_ID;
            currentLoginState.Public_Key = loginAttemptResponseFromCloudObject.Public_Key;
            currentLoginState.Private_Key = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(loginAttemptResponseFromCloudObject.Private_Key, enteredPassword);
            currentLoginState.IdsOfConversationsUserIsParticipantIn = new List<int>(loginAttemptResponseFromCloudObject.IDsOfConversationsUserIsParticipantIn);
            currentLoginState.ConversationInvitations = new List<ConversationInvitation>(loginAttemptResponseFromCloudObject.ConversationInvitations);
        }


        public static int GetCurrentUserAccountID()
        {
            return currentLoginState.Account_ID;
        }


        public static String GetCurrentUserPublicKey()
        {
            return currentLoginState.Public_Key;
        }


        public static List<int> GetIdsOfConversationsCurrentUserIsParticipantIn()
        {
            return currentLoginState.IdsOfConversationsUserIsParticipantIn;
        }


        public static void AddIdToListOfConversationsCurrentUserIsParticipantIn(int conversationIdToAdd)
        {
            currentLoginState.IdsOfConversationsUserIsParticipantIn.Add(conversationIdToAdd);
        }


        public static String GetCurrentUserPrivateKey()
        {
            return currentLoginState.Private_Key;
        }


        public static List<ConversationInvitation> GetConversationInvitations()
        {
            return currentLoginState.ConversationInvitations;
        }


        public static int GetConversationIdOfMostRecentlyLoadedInvitation()
        {
            if (currentLoginState.ConversationInvitations.Count == 0)
                return -1;
            int indexOfMostRecentConversationInvitation = currentLoginState.ConversationInvitations.Count - 1;
            return currentLoginState.ConversationInvitations[indexOfMostRecentConversationInvitation].Conversation_ID;
        }


        public static void AddNewConversationInvitations(ConversationInvitation[] newConversationInvitations)
        {
            currentLoginState.ConversationInvitations.AddRange(newConversationInvitations);
        }


        public static void RemoveConversationInvitation(int conversationIdOfInvitationToRemove)
        {
            int currentIndex = 0;
            foreach (ConversationInvitation currentInvitation in currentLoginState.ConversationInvitations)
            {
                if (currentInvitation.Conversation_ID == conversationIdOfInvitationToRemove)
                {
                    currentLoginState.ConversationInvitations.RemoveAt(currentIndex);
                    return;
                }
                ++currentIndex;
            }
        }
    }
}
