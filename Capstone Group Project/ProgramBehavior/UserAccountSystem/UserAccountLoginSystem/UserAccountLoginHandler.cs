using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.UserAccountSystem.UserAccountLoginSystem
{
    class UserAccountLoginHandler
    {
        public static String errorMessage;


        public static async Task<bool> AttemptToLogUserIn(String enteredUsername, String enteredPassword)
        {
            bool wasLoginAttemptSuccessful = await AttemptToLoadUserAccountDetailsFromTheCloud(enteredUsername, enteredPassword);
            return wasLoginAttemptSuccessful;
        }


        private static async Task<bool> AttemptToLoadUserAccountDetailsFromTheCloud(String enteredUsername, String enteredPassword)
        {
            LogUserIntoAccountRequestObject logUserIntoAccountRequestObject = new LogUserIntoAccountRequestObject(enteredUsername, enteredPassword);
            // We expect the cloud to return the exact same LogUserIntoAccountRequestObject that we originally sent:
            HttpResponseMessage httpResponse = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnHttpResponse(logUserIntoAccountRequestObject, "login.php");
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                String responseJsonString = await httpResponse.Content.ReadAsStringAsync();
                LogUserIntoAccountResponseObject loginAttemptResponseFromCloudObject = JsonConvert.DeserializeObject<LogUserIntoAccountResponseObject>(responseJsonString);
                CurrentLoginState.LoadNewLoginState(loginAttemptResponseFromCloudObject, enteredPassword);
                return true;
            }
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                errorMessage = "ERROR: the entered username does not exist!";
            else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                errorMessage = "ERROR: the entered password was incorrect!";
            else // Otherwise, some other unknown error occured:
                errorMessage = "ERROR: could not log in at this time!";
            return false;
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
