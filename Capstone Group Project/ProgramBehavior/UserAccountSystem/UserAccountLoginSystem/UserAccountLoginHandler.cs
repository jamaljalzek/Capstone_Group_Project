using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.UserAccountSystem.UserAccountLoginSystem
{
    class UserAccountLoginHandler
    {
        public static String errorMessage;
        private static LogUserIntoAccountResponseObject loginAttemptResponseFromCloudObject;
        public static LoginState currentLoginState;


        public static async Task<bool> AttemptToLogUserIn(String enteredUsername, String enteredPassword)
        {
            LogUserIntoAccountRequestObject logUserIntoAccountRequestObject = new LogUserIntoAccountRequestObject(enteredUsername, enteredPassword);
            bool wasLoginAttemptSuccessful = await SendRequestToLogUserInToCloud(logUserIntoAccountRequestObject);
            if (wasLoginAttemptSuccessful)
                LoadUserAccountDetails(enteredPassword);
            return wasLoginAttemptSuccessful;
        }


        private static async Task<bool> SendRequestToLogUserInToCloud(LogUserIntoAccountRequestObject logUserIntoAccountRequestObject)
        {
            // For the time being, while the cloud is not set up we will return true so we can log in:
            return true;

            Object cloudResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsObject(logUserIntoAccountRequestObject);
            // We expect the cloud to return the exact same LogUserIntoAccountRequestObject that we originally sent:
            if (cloudResponseObject is LogUserIntoAccountResponseObject)
            {
                LogUserIntoAccountResponseObject loginAttemptResponseFromCloudObject = cloudResponseObject as LogUserIntoAccountResponseObject;
                if (loginAttemptResponseFromCloudObject.ResultOfRequest.Equals("USERNAME_DOES_NOT_EXIST"))
                {
                    errorMessage = "ERROR: the entered username does not exist!";
                    return false;
                }
                if (loginAttemptResponseFromCloudObject.ResultOfRequest.Equals("PASSWORDS_DO_NOT_MATCH"))
                {
                    errorMessage = "ERROR: the entered password was incorrect!";
                    return false;
                }
                UserAccountLoginHandler.loginAttemptResponseFromCloudObject = loginAttemptResponseFromCloudObject;
                return true;
            }
            throw new Exception("RECEIVED RESPONSE FROM CLOUD THAT WAS NOT A LogUserIntoAccountRequestObject");
        }


        private static void LoadUserAccountDetails(String enteredPassword)
        {
            currentLoginState = new LoginState();
            currentLoginState.Account_ID = loginAttemptResponseFromCloudObject.Account_ID;
            currentLoginState.Public_Key = loginAttemptResponseFromCloudObject.Public_Key;
            currentLoginState.Private_Key = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(loginAttemptResponseFromCloudObject.Private_Key, enteredPassword);
            currentLoginState.IdsOfConversationsUserIsParticipantIn = loginAttemptResponseFromCloudObject.IdsOfConversationsUserIsParticipantIn;
            currentLoginState.ConversationInvitations = loginAttemptResponseFromCloudObject.ConversationInvitations;
            loginAttemptResponseFromCloudObject = null;
        }


        public class LoginState
        {
            public int Account_ID { get; set; } = 0;
            public String Public_Key { get; set; } = null;
            public String Private_Key { get; set; } = null;
            public int[] IdsOfConversationsUserIsParticipantIn { get; set; } = null;
            public ConversationInvitation[] ConversationInvitations { get; set; } = null;
        }


        private static void DeserializationTest()
        {
            LoginState myLoginState = JsonConvert.DeserializeObject<LoginState>("{ 'Account_ID' : 1234, 'Public_Key' : 'MYPUBLICKEY', 'Private_Key' : 'MYPRIVATEKEY', 'IdsOfConversationsUserIsParticipantIn' : [111, 222, 333], 'ConversationInvitations' : [ { 'Conversation_ID' : 777, 'Account_Username_Of_Sender' : 'Jamal' } ] }");
            errorMessage = myLoginState.Account_ID + "\n" +
                myLoginState.Public_Key + "\n" +
                myLoginState.Private_Key + "\n" +
                myLoginState.IdsOfConversationsUserIsParticipantIn.ToString() + "\n" +
                myLoginState.ConversationInvitations.ToString();
            // Place a breakpoint on the below line to inspect the values of the above variables:
            return;
        }
    }
}
