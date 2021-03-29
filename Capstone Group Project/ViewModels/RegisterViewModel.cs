﻿using Capstone_Group_Project.Models;
using Capstone_Group_Project.Program_Behavior.User_Account_System.User_Account_Creation_System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        public Command CreateNewAccountCommand { get; }
        public String EnteredUsername { get; set; }
        public String FirstEnteredPassword { get; set; }
        public String SecondEnteredPassword { get; set; }
        public String DisplayedSuccessOrErrorMessage { get; set; }


        public RegisterViewModel()
        {
            CreateNewAccountCommand = new Command(AttemptToCreateNewUserAccountWithEnteredInformation);
        }


        private async void AttemptToCreateNewUserAccountWithEnteredInformation(object obj)
        {
            if (EnteredUsername == null || FirstEnteredPassword == null || SecondEnteredPassword == null)
                return;
            DisplayedSuccessOrErrorMessage = "Checking entered account details...";
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedSuccessOrErrorMessage");
            // We immediately display the result of attempting to create the new user account,
            // which will either be a success message, or an error message (which specifies what the error is and why):
            DisplayedSuccessOrErrorMessage = await UserAccountCreator.AttemptToCreateNewUserAccount(EnteredUsername, FirstEnteredPassword, SecondEnteredPassword);
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedSuccessOrErrorMessage");
        }


        private void UpdateUserInterfaceElementBoundToGivenVariable(String nameOfVariable)
        {
            // I'm wrapping this method with a more descriptive name for clarity.
            OnPropertyChanged(nameOfVariable);
        }

        private void DeserializationDemo()
        {
            Item item = JsonConvert.DeserializeObject<Item>("{ 'Id' : '" + EnteredUsername + "', 'Text' : '" + FirstEnteredPassword + "', 'Description' : '" + SecondEnteredPassword + "' }");
            DisplayedSuccessOrErrorMessage = item.Id + " " + item.Text + " " + item.Description;
            UpdateUserInterfaceElementBoundToGivenVariable("DisplayedSuccessOrErrorMessage");
        }
    }
}
