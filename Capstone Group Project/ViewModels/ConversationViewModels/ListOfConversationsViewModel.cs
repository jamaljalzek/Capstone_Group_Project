using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class ListOfConversationsViewModel : BaseViewModel
    {
        private static ListOfConversationsViewModel currentInstance = null;
        public ObservableCollection<ConversationListing> ConversationListings { get; }
        public Command<ConversationListing> ConversationListingTapped { get; }
        public Command CreateConversationCommand { get; }
        public static int IdOfConversationListingLastTapped { get; set; }


        public ListOfConversationsViewModel()
        {
            currentInstance = this;
            ConversationListings = new ObservableCollection<ConversationListing>();
            ConversationListingTapped = new Command<ConversationListing>(OnConversationListingSelected);
            CreateConversationCommand = new Command(CreateNewConversation);
            LoadAndDisplayAllConversationListings();
        }


        private void LoadAndDisplayAllConversationListings()
        {
            int[] idsOfConversationsUserIsParticipantIn = CurrentLoginState.GetIdsOfConversationsCurrentUserIsParticipantIn();
            ConversationListings.Clear();
            foreach (int currentConversationId in idsOfConversationsUserIsParticipantIn)
            {
                ConversationListing currentConversationListing = new ConversationListing
                {
                    ConversationId = currentConversationId
                };
                ConversationListings.Add(currentConversationListing);
            }
        }


        async void OnConversationListingSelected(ConversationListing conversationListing)
        {
            if (conversationListing == null)
                return;
            IdOfConversationListingLastTapped = conversationListing.ConversationId;
            // This will push the ConversationListingDetailPage onto the navigation stack
            await Shell.Current.GoToAsync(nameof(IndividualConversationPage));
        }


        private async void CreateNewConversation(object obj)
        {
            await Shell.Current.GoToAsync(nameof(CreateNewConversationPage));
        }


        public static void AddNewConversationListing(int conversationID)
        {
            ConversationListing newConversationListing = new ConversationListing()
            {
                ConversationId = conversationID
            };
            currentInstance.ConversationListings.Add(newConversationListing);
        }
    }
}
