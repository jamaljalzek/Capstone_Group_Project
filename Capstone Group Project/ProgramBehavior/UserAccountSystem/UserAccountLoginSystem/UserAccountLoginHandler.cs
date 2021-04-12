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
            // For the time being, while the cloud is not set up we return true so we can log in:
            return true;

            bool wasLoginAttemptSuccessful = await AttemptToLoadUserAccountDetailsFromTheCloud(enteredUsername, enteredPassword);
            return wasLoginAttemptSuccessful;
        }


        private static async Task<bool> AttemptToLoadUserAccountDetailsFromTheCloud(String enteredUsername, String enteredPassword)
        {
            LogUserIntoAccountRequestObject logUserIntoAccountRequestObject = new LogUserIntoAccountRequestObject(enteredUsername, enteredPassword);
            // We expect the cloud to return the exact same LogUserIntoAccountRequestObject that we originally sent:
            LogUserIntoAccountResponseObject loginAttemptResponseFromCloudObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<LogUserIntoAccountResponseObject>(logUserIntoAccountRequestObject);
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
    }
}
