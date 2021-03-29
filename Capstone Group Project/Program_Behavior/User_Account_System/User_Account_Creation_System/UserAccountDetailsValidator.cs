using Capstone_Group_Project.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Group_Project.Program_Behavior.User_Account_System.User_Account_Creation_System
{
    class UserAccountDetailsValidator
    {
        private static UserAccountDetailsValidator currentInstance;
        public static String errorMessage;

        private String enteredUsername;
        private String firstEnteredPassword;
        private String secondEnteredPassword;


        public static async Task<bool> AreEnteredUserAccountDetailsValid(String enteredUsername, String firstEnteredPassword, String secondEnteredPassword)
        {
            currentInstance = new UserAccountDetailsValidator
            {
                enteredUsername = enteredUsername,
                firstEnteredPassword = firstEnteredPassword,
                secondEnteredPassword = secondEnteredPassword,
            };
            bool areEnteredUserAccountDetailsValid = await currentInstance.AreEnteredUsernameAndPasswordValid();
            currentInstance = null;
            return areEnteredUserAccountDetailsValid;
        }


        private async Task<bool> AreEnteredUsernameAndPasswordValid()
        {
            // When we call IsEnteredUsernameValid(), it runs on a new, separate thread, which is represented by the Task object that it returns.
            // In the meantime, we can check if IsEnteredPasswordValid(), thus running both methods concurrently.
            // Afterwards, we can then await on the Task mentioned above for IsEnteredUsernameValid(),
            // which waits for it to finish if it has not already completed.
            // If it has already completed, then when we call await on the Task, it will return its result immediately:
            Task<bool> checkIfEnteredUsernameIsValidTask = IsEnteredUsernameValid();
            bool isEnteredPasswordValid = IsEnteredPasswordValid();
            bool isEnteredUsernameValid = await checkIfEnteredUsernameIsValidTask;
            return isEnteredUsernameValid && isEnteredPasswordValid;
        }


        private async Task<bool> IsEnteredUsernameValid()
        {
            if (!DoesEnteredUsernameMeetLengthRequirements() || !DoesEnteredUsernameOnlyContainValidCharacters())
                return false;
            // The below needs testing:
            bool isEnteredUsernameAlreadyInUserByAnotherUserAccount = false; // await IsEnteredUsernameAlreadyInUseByAnotherUserAccount();
            return !isEnteredUsernameAlreadyInUserByAnotherUserAccount;
        }


        private bool DoesEnteredUsernameMeetLengthRequirements()
        {
            if (enteredUsername.Length < 10 || enteredUsername.Length > 100)
            {
                errorMessage = "ERROR, the entered username is not between 10 and 100 characters long!";
                return false;
            }
            return true;
        }


        private bool DoesEnteredUsernameOnlyContainValidCharacters()
        {
            foreach (char CurrentCharacter in enteredUsername)
            {
                if (!char.IsLetterOrDigit(CurrentCharacter))
                {
                    if (CurrentCharacter != '-' && CurrentCharacter != '_' && CurrentCharacter != '.')
                    {
                        errorMessage = "ERROR, the entered username does not only contain letters, numbers, dashes, underscores, and periods!";
                        return false;
                    }
                }
            }
            return true;
        }


        private async Task<bool> IsEnteredUsernameAlreadyInUseByAnotherUserAccount()
        {
            Object serverResponseObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsObject(new IsUsernameAlreadyInUseRequestObject(enteredUsername));
            // We expect the cloud to return the exact same IsUsernameAlreadyInUseRequestObject that we originally sent:
            IsUsernameAlreadyInUseRequestObject serverResponseResult = (IsUsernameAlreadyInUseRequestObject)serverResponseObject;
            if (serverResponseResult.IsAccountUsernameAlreadyInUse)
            {
                errorMessage = "ERROR, the entered username is already in use by another user account!";
                return true;
            }
            return false;
        }


        private bool IsEnteredPasswordValid()
        {
            return DoesEnteredPasswordMeetLengthRequirements() && DoesEnteredPasswordMeetSecurityRequirements() && DoEnteredPasswordsMatch();
        }


        private bool DoesEnteredPasswordMeetLengthRequirements()
        {
            // We may want passwords to be a minimum of 16 letters long so that they convert nicely to a 128 bit MD5 hash:
            if (firstEnteredPassword.Length < 10 || firstEnteredPassword.Length > 100)
            {
                errorMessage = "ERROR, the entered passsword is not between 10 and 100 characters long!";
                return false;
            }
            return true;
        }


        private bool DoesEnteredPasswordMeetSecurityRequirements()
        {
            bool containsLowercase = false;
            bool containsUppercase = false;
            bool containsNumber = false;
            foreach (char currentCharacter in enteredUsername)
            {
                if (char.IsLower(currentCharacter))
                    containsLowercase = true;
                if (char.IsUpper(currentCharacter))
                    containsUppercase = true;
                if (char.IsDigit(currentCharacter))
                    containsNumber = true;
            }
            if (!containsLowercase)
            {
                errorMessage = "ERROR, the entered passsword does not contain at least 1 lowercase letter!";
                return false;
            }
            if (!containsUppercase)
            {
                errorMessage = "ERROR, the entered passsword does not contain at least 1 uppercase letter!";
                return false;
            }
            if (!containsNumber)
            {
                errorMessage = "ERROR, the entered passsword does not contain at least 1 digit!";
                return false;
            }
            return true;
        }


        private bool DoEnteredPasswordsMatch()
        {
            if (!firstEnteredPassword.Equals(secondEnteredPassword))
            {
                errorMessage = "ERROR, the entered passswords do not match!";
                return false;
            }
            return true;
        }


        private class IsUsernameAlreadyInUseRequestObject
        {
            // In order for an instance of this class to be properly serialized/deserialized to/from JSON, ALL fields must:
            // 1. Be public.
            // 2. Contain BOTH a getter AND a setter.
            public String TaskRequested { get; set; }
            public String Account_Username { get; set; }
            public bool IsAccountUsernameAlreadyInUse { get; set; }


            public IsUsernameAlreadyInUseRequestObject(String enteredUsername)
            {
                this.TaskRequested = "CHECK_IF_ACCOUNT_USERNAME_IS_IN_USE";
                this.Account_Username = enteredUsername;
                this.IsAccountUsernameAlreadyInUse = false;
            }
        }
    }
}
