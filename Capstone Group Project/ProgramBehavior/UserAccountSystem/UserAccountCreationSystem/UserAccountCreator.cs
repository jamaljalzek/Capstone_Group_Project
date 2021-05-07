using Capstone_Group_Project.Models;
using Capstone_Group_Project.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Capstone_Group_Project.Program_Behavior.User_Account_System.User_Account_Creation_System
{
    class UserAccountCreator
    {
        public static async Task<String> AttemptToCreateNewUserAccount(String enteredUsername, String firstEnteredPassword, String secondEnteredPassword)
        {
            bool areEnteredUserAccountDetailsValid = UserAccountDetailsValidator.AreEnteredUserAccountDetailsValid(enteredUsername, firstEnteredPassword, secondEnteredPassword);
            if (!areEnteredUserAccountDetailsValid)
                return UserAccountDetailsValidator.errorMessage;
            String resultOfAttempt = await SendRequestToCreateNewUserAccountToCloud(enteredUsername, firstEnteredPassword);
            return resultOfAttempt;
        }


        private static async Task<String> SendRequestToCreateNewUserAccountToCloud(String enteredUsername, String enteredPassword)
        {
            CreateNewUserAccountRequestObject createNewUserAccountRequestObject = new CreateNewUserAccountRequestObject(enteredUsername, enteredPassword);
            HttpStatusCode resultStatusCode = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnHttpResponseCode(createNewUserAccountRequestObject, "create_account.php");
            if (resultStatusCode == HttpStatusCode.Created)
                return "The new user account has been successfully created!";
            if (resultStatusCode == HttpStatusCode.Conflict)
                return "ERROR, the entered username is already in use by another user account!";
            // If for some unknown reason we receive another status code, such as in the event of an error:
            return "ERROR: the user account was not created!";
        }


        private class CreateNewUserAccountRequestObject : CloudCommunicationObject
        {
            public String Account_Username { get; set; } = null;
            public String Account_Password_Hashcode { get; set; } = null;
            public String Public_Key { get; set; } = null;
            public String Private_Key { get; set; } = null;


            public CreateNewUserAccountRequestObject(String enteredUsername, String enteredPassword)
            {
                this.TaskRequested = "CREATE_NEW_ACCOUNT";
                this.Account_Username = enteredUsername;
                this.Account_Password_Hashcode = Hashing.ConvertPasswordStringIntoSha256HashCodeBase64String(enteredPassword);
                String publicAndPrivateKey = AsymmetricEncryption.CreateNewPublicAndPrivateKeyAndReturnAsXmlString();
                this.Private_Key = SymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String(publicAndPrivateKey, enteredPassword);
                this.Public_Key = AsymmetricEncryption.ExtractPublicKeyAndReturnAsXmlString(publicAndPrivateKey);
            }
        }
    }
}
