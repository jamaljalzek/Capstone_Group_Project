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
        private static UserAccountCreator currentInstance;
        public static String errorMessage;

        private String enteredUsername;
        private String firstEnteredPassword;
        private String secondEnteredPassword;
        private byte[] publicKey;
        private byte[] privateKey;


        public static async Task <String> AttemptToCreateNewUserAccount(String enteredUsername, String firstEnteredPassword, String secondEnteredPassword)
        {
            currentInstance = new UserAccountCreator
            {
                enteredUsername = enteredUsername,
                firstEnteredPassword = firstEnteredPassword,
                secondEnteredPassword = secondEnteredPassword,
            };
            currentInstance = null;
            bool areEnteredUserAccountDetailsValid = await UserAccountDetailsValidator.AreEnteredUserAccountDetailsValid(enteredUsername, firstEnteredPassword, secondEnteredPassword);
            if (areEnteredUserAccountDetailsValid)
            {
                return "Account has been successfully created!";
            }
            else
                return UserAccountDetailsValidator.errorMessage;
        }


        private void CreateNewUserAccount()
        {

        }


        private void CreateNewPublicAndPrivateKeyPair()
        {
            RSA rsa = RSA.Create();
            RSAParameters rsaParameters = rsa.ExportParameters(true);
        }


        private class CreateNewUserAccountRequestObject
        {
            public CreateNewUserAccountRequestObject()
            {

            }
        }
    }
}
