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
        private String enteredPassword;
        private String publicAndPrivateKey;
        private String justThePublicKey;


        public static async Task <String> AttemptToCreateNewUserAccount(String enteredUsername, String firstEnteredPassword, String secondEnteredPassword)
        {
            bool areEnteredUserAccountDetailsValid = await UserAccountDetailsValidator.AreEnteredUserAccountDetailsValid(enteredUsername, firstEnteredPassword, secondEnteredPassword);
            if (!areEnteredUserAccountDetailsValid)
            //    return UserAccountDetailsValidator.errorMessage;
            CreateNewUserAccount(enteredUsername, firstEnteredPassword);
            return "Account has been successfully created!";
        }


        private static void CreateNewUserAccount(String enteredUsername, String firstEnteredPassword)
        {
            currentInstance = new UserAccountCreator
            {
                enteredUsername = enteredUsername,
                enteredPassword = firstEnteredPassword
            };
            currentInstance.CreateNewPublicAndPrivateKeyPair();
            currentInstance = null;
        }


        private void CreateNewPublicAndPrivateKeyPair()
        {
            publicAndPrivateKey = AsymmetricEncryption.CreateNewPublicAndPrivateKeyAndReturnAsXmlString();
            justThePublicKey = AsymmetricEncryption.ExtractPublicKeyAndReturnAsXmlString(publicAndPrivateKey);
            //AsymmetricEncryptionAndDecryptionTest();
            //SymmetricEncryptionAndDecryptionTest();
        }


        private void AsymmetricEncryptionAndDecryptionTest()
        {
            // Encryption test:
            String ciphertext = AsymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String("THIS is AN encryption TEST!", justThePublicKey);
            // Decryption test:
            String plaintext = AsymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(ciphertext, publicAndPrivateKey);
            // Place a breakpoint on the below line to inspect the values of the above two variables:
            return;
        }


        private void SymmetricEncryptionAndDecryptionTest()
        {
            // Encryption test:
            String ciphertext = SymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String("THIS is AN encryption TEST!", enteredPassword);
            // Decryption test:
            String plaintext = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(ciphertext, enteredPassword);
            // Place a breakpoint on the below line to inspect the values of the above two variables:
            return;
        }


        private class CreateNewUserAccountRequestObject
        {
            public CreateNewUserAccountRequestObject()
            {

            }
        }
    }
}
