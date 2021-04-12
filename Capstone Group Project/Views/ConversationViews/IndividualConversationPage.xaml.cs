using Capstone_Group_Project.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone_Group_Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndividualConversationPage : ContentPage
    {
        public IndividualConversationPage()
        {
            InitializeComponent();
            this.BindingContext = new IndividualConversationViewModel();
        }


        private void MessagesView_SizeChanged(object sender, EventArgs e)
        {
            // This method only gets called once the initial set of messages are loaded into the MessagesView.
            // This method then scrolls the MessagesView down all the way to the very last message, at the index specificed below:
            int bottomViewIndexOfMostRecentMessage = IndividualConversationViewModel.NUMBER_OF_MESSAGES_TO_LOAD_AT_A_TIME - 1;
            MessagesView.ScrollTo(bottomViewIndexOfMostRecentMessage);
            // Thankfully, whenever we add new messages to the MessagesView, like when we load more messages from further back in time,
            // this method does not activate again, meaning it won't scroll down to the latest message when the user was trying to scroll up
            // to load older messages.
            // Trying to rely on the "ChildAdded" event does not work as we need it to: it will scroll down to the bottom message, however,
            // when we try to scroll up, it strangely automatically scrolls back down, despite no new elements being added.
            // Somehow, tapping events triggers it is my guess.
        }
    }
}