using Capstone_Group_Project.Models;
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
        public Command SendNewInviteCommand { get; set; } = null;
        public String EnteredMessage { get; set; } = "";
        public Command SendMessageCommand { get; set; } = null;
        public Command LoadMoreMessagesCommand { get; set; } = null;
        public ObservableCollection<Message> LoadedMessages { get; set; } = null;

        private int LoadedMessagesEndingMessageNumberInclusive = 0;

        public const int NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME = 25;


        public IndividualConversationViewModel()
        {
            CurrentConverstionIdNumber = ListOfConversationsViewModel.IdOfConversationListingLastTapped;
            UpdateUserInterfaceElementBoundToGivenVariable("CurrentConversationIdNumber");
            CurrentConversationState.SetCurrentConversationID(CurrentConverstionIdNumber);
            SendNewInviteCommand = new Command(InviteNewParticipant);
            SendMessageCommand = new Command(SendEnteredMessageToAllConversationParticipants);
            LoadMoreMessagesCommand = new Command(DisplayAnotherSetOfMessagesForThisConversation);
            DisplayInitialSetOfMessagesForThisConversation();
        }


        private async void InviteNewParticipant(object obj)
        {
            await Shell.Current.GoToAsync(nameof(InviteNewParticipantPage));
        }


        private async void SendEnteredMessageToAllConversationParticipants()
        {
            if (EnteredMessage.Length == 0)
                return;
            DateTime currentDateAndTime = DateTime.Now;
            //SendConversationMessageHandler.SendIndividualMessageToAllConversationParticipants(EnteredMessage, currentDateAndTime);
            DisplaySentMessageOnOurEnd(currentDateAndTime);
            EnteredMessage = "";
            UpdateUserInterfaceElementBoundToGivenVariable("EnteredMessage");
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
            // Scroll to the bottom of the conversation so the latest message appears on screen:
            IndividualConversationPage.ScrollToSpecifiedMessageNumberInMessagesView(LoadedMessages.Count);
        }


        private async void DisplayInitialSetOfMessagesForThisConversation()
        {
            LoadedMessagesEndingMessageNumberInclusive = NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME;
            Message[] listOfLoadedMessages = await CurrentConversationState.LoadInitialMessagesForCurrentConversation(LoadedMessagesEndingMessageNumberInclusive);
            LoadedMessages = new ObservableCollection<Message>(listOfLoadedMessages);
        }


        private async void DisplayAnotherSetOfMessagesForThisConversation()
        {
            int StartingNumberForNextSetOfMessagesToLoadInclusive = LoadedMessagesEndingMessageNumberInclusive + 1;
            LoadedMessagesEndingMessageNumberInclusive += NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME;
            Message[] listOfRecentlyLoadedMessages = await CurrentConversationState.LoadRangeOfMessagesForCurrentConversation(StartingNumberForNextSetOfMessagesToLoadInclusive, LoadedMessagesEndingMessageNumberInclusive);
            for (int currentIndex = listOfRecentlyLoadedMessages.Length - 1; currentIndex >= 0; --currentIndex)
            {
                Message currentRecentlyLoadedMessage = listOfRecentlyLoadedMessages[currentIndex];
                // Add each message to the front/top of the list, not the buttom:
                LoadedMessages.Insert(0, currentRecentlyLoadedMessage);
            }
        }
    }
}
