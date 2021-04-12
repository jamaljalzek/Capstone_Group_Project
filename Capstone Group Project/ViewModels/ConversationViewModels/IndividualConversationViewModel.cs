using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.ConversationSystem.LoadingIndividualConversationSystem;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class IndividualConversationViewModel : BaseViewModel
    {
        public int CurrentConverstionIdNumber { get; set; } = 0;
        public String EnteredMessage { get; set; } = null;
        public Command SendMessageCommand { get; set; } = null;
        public Command LoadMoreMessagesCommand { get; set; } = null;
        public ObservableCollection<Message> LoadedMessages { get; set; } = null;
        private int LoadedMessagesEndingMessageNumberInclusive = 0;
        public const int NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME = 25;


        public IndividualConversationViewModel()
        {
            CurrentConverstionIdNumber = ListOfConversationsViewModel.IdOfConversationListingLastTapped;
            UpdateUserInterfaceElementBoundToGivenVariable("CurrentConversationIdNumber");
            SendMessageCommand = new Command(SendEnteredMessageToAllConversationParticipants);
            LoadMoreMessagesCommand = new Command(DisplayAnotherSetOfMessagesForThisConversation);
            DisplayInitialSetOfMessagesForThisConversation();
        }


        private async void SendEnteredMessageToAllConversationParticipants()
        {
            Message newMessage = new Message()
            {
                MessageSenderUsername = "Jamal",
                TimeAndDateMessageWasSent = "4/9/2021",
                MessageBody = "TEST MESSAGE #" + 999 + " FOR CONVERSATION #" + CurrentConverstionIdNumber + "blah blah blah blah blah blah blah blah"
            };
            LoadedMessages.Add(newMessage);
        }


        private async void DisplayInitialSetOfMessagesForThisConversation()
        {
            LoadedMessagesEndingMessageNumberInclusive = NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME;
            Message[] listOfLoadedMessages = await CurrentConversationState.LoadInitialMessagesForGivenConversation(CurrentConverstionIdNumber, LoadedMessagesEndingMessageNumberInclusive);
            LoadedMessages = new ObservableCollection<Message>(listOfLoadedMessages);
        }


        private async void DisplayAnotherSetOfMessagesForThisConversation()
        {
            int StartingNumberForNextSetOfMessagesToLoadInclusive = LoadedMessagesEndingMessageNumberInclusive + 1;
            LoadedMessagesEndingMessageNumberInclusive += NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME;
            Message[] listOfRecentlyLoadedMessages = await CurrentConversationState.LoadRangeOfMessagesForGivenConversation(StartingNumberForNextSetOfMessagesToLoadInclusive, LoadedMessagesEndingMessageNumberInclusive, CurrentConverstionIdNumber);
            for (int currentIndex = listOfRecentlyLoadedMessages.Length - 1; currentIndex >= 0; --currentIndex)
            {
                Message currentRecentlyLoadedMessage = listOfRecentlyLoadedMessages[currentIndex];
                // Add each message to the front/top of the list, not the buttom:
                LoadedMessages.Insert(0, currentRecentlyLoadedMessage);
            }
        }
    }
}
