using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using System;

namespace Capstone_Group_Project.ProgramBehavior.UserAccountSystem
{
    public class CurrentLoginState
    {
        private static CurrentLoginState currentLoginState;

        public int Account_ID { get; set; } = 0;
        public String Public_Key { get; set; } = null;
        public String Private_Key { get; set; } = null;
        public int[] IdsOfConversationsUserIsParticipantIn { get; set; } = null;
        public ConversationInvitation[] ConversationInvitations { get; set; } = null;


        public static void LoadNewLoginState(LogUserIntoAccountResponseObject loginAttemptResponseFromCloudObject, String enteredPassword)
        {
            currentLoginState = new CurrentLoginState();
            currentLoginState.Account_ID = loginAttemptResponseFromCloudObject.Account_ID;
            currentLoginState.Public_Key = loginAttemptResponseFromCloudObject.Public_Key;
            currentLoginState.Private_Key = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(loginAttemptResponseFromCloudObject.Private_Key, enteredPassword);
            currentLoginState.IdsOfConversationsUserIsParticipantIn = loginAttemptResponseFromCloudObject.IdsOfConversationsUserIsParticipantIn;
            currentLoginState.ConversationInvitations = loginAttemptResponseFromCloudObject.ConversationInvitations;
        }


        public static int GetCurrentUserAccountID()
        {
            return currentLoginState.Account_ID;
        }


        public static String GetCurrentUserPublicKey()
        {
            return currentLoginState.Public_Key;
        }
    }
}
