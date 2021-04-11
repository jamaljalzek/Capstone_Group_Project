using Capstone_Group_Project.Views;
using Xamarin.Forms;

namespace Capstone_Group_Project.ViewModels
{
    class ListOfConversationsViewModel : BaseViewModel
    {
        public Command CreateConversationCommand { get; }


        public ListOfConversationsViewModel()
        {
            CreateConversationCommand = new Command(CreateNewConversation);
        }


        private async void CreateNewConversation(object obj)
        {
            await Shell.Current.GoToAsync(nameof(CreateNewConversationPage));
        }
    }
}
