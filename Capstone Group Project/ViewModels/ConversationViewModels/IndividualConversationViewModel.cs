using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.ConversationReceivingMessageSystem;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.ConversationSendingMessageSystem;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem;
using Capstone_Group_Project.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class IndividualConversationViewModel : BaseViewModel
    {
        public int CurrentConverstionIdNumber { get; set; } = 0;
        public Command CheckForNewMessagesCommand { get; set; } = null;
        public Command SendNewInviteCommand { get; set; } = null;
        public String EnteredMessage { get; set; } = "";
        public Command SendMessageCommand { get; set; } = null;
        public Command LoadMoreMessagesCommand { get; set; } = null;
        public ObservableCollection<Message> LoadedMessages { get; set; } = null;

        private int LoadedMessagesStartingMessageNumberInclusive = 0;

        private int LoadedMessagesEndingMessageNumberInclusive = 0;

        public const int NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME = 25;


        public IndividualConversationViewModel()
        {
            CurrentConverstionIdNumber = ListOfConversationsViewModel.IdOfConversationListingLastTapped;
            UpdateUserInterfaceElementBoundToGivenVariable("CurrentConversationIdNumber");
            CurrentConversationState.SetCurrentConversationID(CurrentConverstionIdNumber);
            CheckForNewMessagesCommand = new Command(CheckForNewMessages);
            SendNewInviteCommand = new Command(InviteNewParticipant);
            SendMessageCommand = new Command(SendEnteredMessageToAllConversationParticipants);
            LoadMoreMessagesCommand = new Command(DisplayAnotherSetOfOlderMessagesForThisConversation);
            DisplayInitialSetOfMessagesForThisConversation();
        }


        private async void CheckForNewMessages(object obj)
        {
            Message[] listOfRecentlyLoadedMessages = await LoadConversationMessagesHandler.LoadAnyNewMessagesForCurrentConversation(LoadedMessagesEndingMessageNumberInclusive);
            for (int currentIndex = 0; currentIndex < listOfRecentlyLoadedMessages.Length; ++currentIndex)
            {
                Message currentRecentlyLoadedMessage = listOfRecentlyLoadedMessages[currentIndex];
                LoadedMessages.Add(currentRecentlyLoadedMessage);
            }
            if (listOfRecentlyLoadedMessages.Length > 0)
                LoadedMessagesEndingMessageNumberInclusive = listOfRecentlyLoadedMessages[listOfRecentlyLoadedMessages.Length - 1].Message_Number;
            UpdateUserInterfaceElementBoundToGivenVariable("LoadedMessages");
            IndividualConversationPage.ScrollToSpecifiedMessageNumberInMessagesView(LoadedMessages.Count);
        }


        private async void InviteNewParticipant(object obj)
        {
            await Shell.Current.Navigation.PushModalAsync(new InviteNewParticipantPage());
        }


        private async void SendEnteredMessageToAllConversationParticipants()
        {
            if (EnteredMessage.Length == 0)
                return;
            DateTime currentDateAndTime = DateTime.Now;
            bool resultOfAttempt = await SendConversationMessageHandler.SendIndividualMessageToAllConversationParticipants(EnteredMessage, currentDateAndTime);
            if (resultOfAttempt)
            {
                CheckForNewMessages(null);
                EnteredMessage = "";
                UpdateUserInterfaceElementBoundToGivenVariable("EnteredMessage");
            }
        }


        private void DisplaySentMessageOnOurEnd(DateTime currentDateAndTime)
        {
            Message newMessage = new Message()
            {
                MessageSenderUsername = "Me",
                TimeAndDateMessageWasSent = currentDateAndTime.ToString(),
                MessageBody = EnteredMessage
            };
            LoadedMessages.Add(newMessage);
            UpdateUserInterfaceElementBoundToGivenVariable("LoadedMessages");
            // Scroll to the bottom of the conversation so the latest message appears on screen:
            IndividualConversationPage.ScrollToSpecifiedMessageNumberInMessagesView(LoadedMessages.Count);
        }


        private async void DisplayInitialSetOfMessagesForThisConversation()
        {
            Message[] listOfLoadedMessages = await LoadConversationMessagesHandler.LoadInitialMessagesForCurrentConversation(NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME);
            LoadedMessages = new ObservableCollection<Message>(listOfLoadedMessages);
            if (listOfLoadedMessages.Length > 0)
            {
                LoadedMessagesStartingMessageNumberInclusive = listOfLoadedMessages[0].Message_Number;
                LoadedMessagesEndingMessageNumberInclusive = listOfLoadedMessages[listOfLoadedMessages.Length - 1].Message_Number;
            }
            UpdateUserInterfaceElementBoundToGivenVariable("LoadedMessages");
            IndividualConversationPage.ScrollToSpecifiedMessageNumberInMessagesView(LoadedMessages.Count);
        }


        private async void DisplayAnotherSetOfOlderMessagesForThisConversation()
        {
            int StartingNumberForNextSetOfMessagesToLoadInclusive = LoadedMessagesStartingMessageNumberInclusive - NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME - 1;
            int EndingNumberForNextSetOfMessagesToLoadInclusive = StartingNumberForNextSetOfMessagesToLoadInclusive + NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME;
            Message[] listOfRecentlyLoadedMessages = await LoadConversationMessagesHandler.LoadRangeOfMessagesForCurrentConversation(StartingNumberForNextSetOfMessagesToLoadInclusive, EndingNumberForNextSetOfMessagesToLoadInclusive);
            for (int currentIndex = listOfRecentlyLoadedMessages.Length - 1; currentIndex >= 0; --currentIndex)
            {
                Message currentRecentlyLoadedMessage = listOfRecentlyLoadedMessages[currentIndex];
                // Add each message to the front/top of the list, not the buttom:
                LoadedMessages.Insert(0, currentRecentlyLoadedMessage);
            }
            if (listOfRecentlyLoadedMessages.Length > 0)
                LoadedMessagesStartingMessageNumberInclusive = listOfRecentlyLoadedMessages[0].Message_Number;
            UpdateUserInterfaceElementBoundToGivenVariable("LoadedMessages");
        }
    }
}
