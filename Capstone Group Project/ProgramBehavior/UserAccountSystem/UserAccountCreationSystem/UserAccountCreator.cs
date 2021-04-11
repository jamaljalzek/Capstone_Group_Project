using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using System;
using System.Threading.Tasks;

namespace Capstone_Group_Project.Program_Behavior.User_Account_System.User_Account_Creation_System
{
    class UserAccountCreator
    {
        public static async Task<String> AttemptToCreateNewUserAccount(String enteredUsername, String firstEnteredPassword, String secondEnteredPassword)
        {
            bool areEnteredUserAccountDetailsValid = await UserAccountDetailsValidator.AreEnteredUserAccountDetailsValid(enteredUsername, firstEnteredPassword, secondEnteredPassword);
            if (!areEnteredUserAccountDetailsValid)
                return UserAccountDetailsValidator.errorMessage;
            bool wasNewAccountSuccessfullyCreated = await SendRequestToCreateNewUserAccountToCloud(enteredUsername, firstEnteredPassword);
            if (!wasNewAccountSuccessfullyCreated)
                return "ERROR: the user account was not created!";
            return "The new user account has been successfully created!";
        }


        private static async Task<bool> SendRequestToCreateNewUserAccountToCloud(String enteredUsername, String enteredPassword)
        {
            CreateNewUserAccountRequestObject createNewUserAccountRequestObject = new CreateNewUserAccountRequestObject(enteredUsername, enteredPassword);
            // We expect the cloud to return the exact same CreateNewUserAccountRequestObject that we originally sent:
            CreateNewUserAccountRequestObject loginAttemptResponseFromCloudObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<CreateNewUserAccountRequestObject>(createNewUserAccountRequestObject);
            return loginAttemptResponseFromCloudObject.ResultOfRequest.Equals("ACCOUNT_CREATED");
        }
    }
}
