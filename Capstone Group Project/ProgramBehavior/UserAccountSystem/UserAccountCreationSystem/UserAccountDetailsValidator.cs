using System;

namespace Capstone_Group_Project.Program_Behavior.User_Account_System.User_Account_Creation_System
{
    class UserAccountDetailsValidator
    {
        public static String errorMessage;

        private String enteredUsername;
        private String firstEnteredPassword;
        private String secondEnteredPassword;


        public static bool AreEnteredUserAccountDetailsValid(String enteredUsername, String firstEnteredPassword, String secondEnteredPassword)
        {
            // We store the entered details in the below object, which we then throw away immediately after this method terminates.
            // This is so we avoid storing any passwords in plaintext in memory, which increases security.
            UserAccountDetailsValidator currentInstance = new UserAccountDetailsValidator
            {
                enteredUsername = enteredUsername,
                firstEnteredPassword = firstEnteredPassword,
                secondEnteredPassword = secondEnteredPassword,
            };
            return currentInstance.IsEnteredUsernameValid() && currentInstance.IsEnteredPasswordValid();
        }


        private bool IsEnteredUsernameValid()
        {
            return DoesEnteredUsernameMeetLengthRequirements() && DoesEnteredUsernameOnlyContainValidCharacters();
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


        private bool IsEnteredPasswordValid()
        {
            return DoEnteredPasswordsMatch() && DoesEnteredPasswordMeetLengthRequirements() && DoesEnteredPasswordMeetSecurityRequirements();
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


        private bool DoesEnteredPasswordMeetLengthRequirements()
        {
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
            foreach (char currentCharacter in firstEnteredPassword)
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
    }
}
