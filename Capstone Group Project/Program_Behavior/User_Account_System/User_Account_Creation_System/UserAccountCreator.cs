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
            // Version 2.0: the cloud should respond and confirm that the account has indeed been created:
            Object serverResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsObject(createNewUserAccountRequestObject);
        }


        private class CreateNewUserAccountRequestObject
        {
            // In order for an instance of this class to be properly serialized/deserialized to/from JSON, ALL fields must:
            // 1. Be public.
            // 2. Contain BOTH a getter AND a setter.
            public String TaskRequested { get; set; }
            public String Account_Username { get; set; }
            public String Account_Password_Hashcode { get; set; }
            public String Public_Key { get; set; }
            public String Private_Key { get; set; }


            public CreateNewUserAccountRequestObject(String enteredUsername, String enteredPassword)
            {
                this.TaskRequested = "CREATE_NEW_USER_ACCOUNT";
                this.Account_Username = enteredUsername;
                this.Account_Password_Hashcode = Hashing.ConvertPasswordStringIntoSha256HashCodeBase64String(enteredPassword);
                String publicAndPrivateKey = AsymmetricEncryption.CreateNewPublicAndPrivateKeyAndReturnAsXmlString();
                this.Private_Key = SymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String(publicAndPrivateKey, enteredPassword);
                this.Public_Key = AsymmetricEncryption.ExtractPublicKeyAndReturnAsXmlString(publicAndPrivateKey);
            }
        }
    }
}
