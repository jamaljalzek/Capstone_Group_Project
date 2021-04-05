using Capstone_Group_Project.Views;
using System;
using Xamarin.Forms;

namespace Capstone_Group_Project
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ListOfConversationsPage), typeof(ListOfConversationsPage));
            Routing.RegisterRoute(nameof(CreateNewConversationPage), typeof(CreateNewConversationPage));
            this.CurrentItem = new LoginPage();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
