using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Group_Project.Program_Behavior.User_Account_System.User_Account_Creation_System
{
    class UserAccountCreator
    {
        public static String errorMessage;


        public static async Task <String> AttemptToCreateNewUserAccount(String enteredUsername, String firstEnteredPassword, String secondEnteredPassword)
        {
            bool areEnteredUserAccountDetailsValid = await UserAccountDetailsValidator.AreEnteredUserAccountDetailsValid(enteredUsername, firstEnteredPassword, secondEnteredPassword);
            if (!areEnteredUserAccountDetailsValid)
                return UserAccountDetailsValidator.errorMessage;
            CreateNewUserAccountRequestObject createNewUserAccountRequestObject = new CreateNewUserAccountRequestObject(enteredUsername, firstEnteredPassword);
            SendRequestToCreateNewUserAccountToCloud(createNewUserAccountRequestObject);
            return "The new user account has been successfully created!";
        }


        private static async void SendRequestToCreateNewUserAccountToCloud(CreateNewUserAccountRequestObject createNewUserAccountRequestObject)
        {
            Object cloudResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsObject(createNewUserAccountRequestObject);
            // We expect the cloud to return the exact same CreateNewUserAccountRequestObject that we originally sent:
            if (cloudResponseObject is CreateNewUserAccountRequestObject)
            {
                CreateNewUserAccountRequestObject loginAttemptResponseFromCloudObject = cloudResponseObject as CreateNewUserAccountRequestObject;
                if (!loginAttemptResponseFromCloudObject.ResultOfRequest.Equals("ACCOUNT_CREATED"))
                {
                    errorMessage = "ERROR: the user account was not created!";
                }
            }
            throw new Exception("RECEIVED RESPONSE FROM CLOUD THAT WAS NOT A CreateNewUserAccountRequestObject");
        }
    }
}
