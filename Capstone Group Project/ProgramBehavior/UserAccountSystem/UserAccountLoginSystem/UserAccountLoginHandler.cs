using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using System;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.UserAccountSystem.UserAccountLoginSystem
{
    class UserAccountLoginHandler
    {
        public static String errorMessage;


        public static async Task<bool> AttemptToLogUserIn(String enteredUsername, String enteredPassword)
        {
            // Debugging account:
            if (enteredUsername.Equals("test"))
                return true;
            bool wasLoginAttemptSuccessful = await AttemptToLoadUserAccountDetailsFromTheCloud(enteredUsername, enteredPassword);
            return wasLoginAttemptSuccessful;
        }


        private static async Task<bool> AttemptToLoadUserAccountDetailsFromTheCloud(String enteredUsername, String enteredPassword)
        {
            LogUserIntoAccountRequestObject logUserIntoAccountRequestObject = new LogUserIntoAccountRequestObject(enteredUsername, enteredPassword);
            // We expect the cloud to return the exact same LogUserIntoAccountRequestObject that we originally sent:
            LogUserIntoAccountResponseObject loginAttemptResponseFromCloudObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LogUserIntoAccountResponseObject>(logUserIntoAccountRequestObject, "login.php");
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
            CurrentLoginState.LoadNewLoginState(loginAttemptResponseFromCloudObject, enteredPassword);
            return true;
        }


        private class LogUserIntoAccountRequestObject : CloudCommunicationObject
        {
            public String Account_Username { get; set; } = null;
            public String Account_Password_Hashcode { get; set; } = null;


            public LogUserIntoAccountRequestObject(String enteredUsername, String enteredPassword)
            {
                this.TaskRequested = "LOG_INTO_ACCOUNT";
                this.Account_Username = enteredUsername;
                this.Account_Password_Hashcode = Hashing.ConvertPasswordStringIntoSha256HashCodeBase64String(enteredPassword);
            }
        }
    }
}
