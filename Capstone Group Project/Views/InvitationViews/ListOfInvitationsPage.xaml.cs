using Capstone_Group_Project.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone_Group_Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListOfInvitationsPage : ContentPage
    {
        public ListOfInvitationsPage()
        {
            InitializeComponent();
            this.BindingContext = new ListOfInvitationsViewModel();
        }
    }
}